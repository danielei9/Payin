using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments.Liquidation;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Liquidation", "TimerCreate")]
	public class TimerLiquidationCreateHandler :
		IServiceBaseHandler<TimerLiquidationCreateArguments>
	{
		[Dependency] public IEntityRepository<Ticket> TicketRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TimerLiquidationCreateArguments arguments)
		{
			var now = DateTime.Now;

			var liquidations = (await TicketRepository.GetAsync())
				.Select(x => new
				{
					Id = x.ConcessionId,
					Concession = x.Concession,
					FirstPaymentDate = x.Payments.Min(y => y.Date),
					Payments = x.Payments
						.Where(y =>
							y.LiquidationId == null &&
							y.State == PaymentState.Active
						)
						.Union(x.Payments.Where(z =>
							z.LiquidationId == null &&
							z.State == PaymentState.Active &&
							z.Ticket.LiquidationConcession == x.Concession)
						),
					Recharges = x.Recharges
						.Where(y =>
							y.LiquidationId == null
						)
				})
				.GroupBy(x => new { x.Id })
				.Select(x => new
				{
					Concession = x
						.Select(y => y.Concession)
						.FirstOrDefault(),
					LastLiquidationDate = x
						.SelectMany(y => y.Concession.Liquidations)
						.Max(y => (DateTime?)y.Until) ??  // Fecha de cierre de última liquidation
						x.Min(y => y.FirstPaymentDate), // Primer pago
					PaymentTotal = x
						.SelectMany(y => y.Payments)
						.Sum(y => (decimal?)y.Amount) ?? 0m,
					RechargesTotal = x
						.SelectMany(y => y.Recharges)
						.Sum(y => (decimal?)y.Amount) ?? 0m,
					Payin = x
						.SelectMany(y => y.Payments)
						.Sum(y => (decimal?)y.Payin) ?? 0m,
					Payments = x
						.SelectMany(y => y.Payments),
					Recharges = x
						.SelectMany(y => y.Recharges)
				})
				.Where(x =>
					x.PaymentTotal - x.RechargesTotal > x.Concession.LiquidationAmountMin
				);

			foreach (var values in liquidations)
			{
				var liquidation = new Liquidation
				{
					State = LiquidationState.Closed,
					Until = now.ToUTC(),
					Since = values.LastLiquidationDate,
					TotalQuantity = values.PaymentTotal - values.RechargesTotal,
					PayinQuantity = values.Payin,
					PaidQuantity = values.PaymentTotal - values.RechargesTotal - values.Payin,
					PaymentDate = null,
					RequestDate = values.Concession.LiquidationRequestDate,
					Concession = values.Concession,
					Payments = values.Payments.ToList() as IList<Payment>,
					Recharges = values.Recharges.ToList() as IList<Recharge>
				};

				values.Concession.LiquidationRequestDate = null;
				values.Concession.Liquidations.Add(liquidation);
			}

			return null;
		}
		#endregion ExecuteAsync
	}
}
