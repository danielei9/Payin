using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class LiquidationCreateHandler :
		IServiceBaseHandler<LiquidationCreateArguments>
	{
		[Dependency] public IEntityRepository<Payment> PaymentRepository { get; set; }
		[Dependency] public IEntityRepository<Liquidation> Repository { get; set; }
		[Dependency] public AccountLineGetAllUnliquidatedHandler PaymentUnliquidatedHandler { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(LiquidationCreateArguments arguments)
		{
			var now = DateTime.UtcNow;

			var items = (await PaymentUnliquidatedHandler.ExecuteAsync(arguments))
				.Data
				.GroupBy(x => x.TicketConcessionId)
				.Select(x => new {
					ConcessionId = x.Key,
					Ids = x.Select(y => y.Id)
				});

			foreach (var item in items)
			{
				var payments = (await PaymentRepository.GetAsync())
					.Where(x => item.Ids.Contains(x.Id))
					.ToList();

				var since = payments
					.Min(x => x.Date);
				var until = payments
					.Max(x => x.Date);

				await Repository.AddAsync(new Liquidation
				{
					Payments = payments,
					TotalQuantity = payments.Sum(y => (Decimal?)y.Amount ?? 0),
					PayinQuantity = payments.Sum(y => (Decimal?)y.Payin ?? 0),
					PaidQuantity = payments.Sum(y => (Decimal?)y.Amount ?? 0) - payments.Sum(y => (Decimal?)y.Payin ?? 0),
					State = LiquidationState.Closed,
					Since = since,
					Until = until,
					PaymentDate = null,
					RequestDate = now,
					PaidBank = false,
					PaidTPV = false,
					ConcessionId = item.ConcessionId
				});
			}

			return null;
		}
		#endregion ExecuteAsync
	}
}
