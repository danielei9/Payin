using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments.Liquidation;
using PayIn.Application.Dto.Payments.Results.Liquidation;
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
	
	public class LiquidationGetCreateHandler :
		IQueryBaseHandler<LiquidationGetCreateArguments, LiquidationGetCreateResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<Payment> Repository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<SystemCardMember> SystemCardMemberRepository { get; set; }
		[Dependency] public IEntityRepository<Liquidation> LiquidationRepository { get; set; }
		[Dependency] public IEntityRepository<Event> EventRepository { get; set; }


		#region ExecuteAsync
		public async Task<ResultBase<LiquidationGetCreateResult>> ExecuteAsync(LiquidationGetCreateArguments arguments)
		{
            var now = DateTime.UtcNow;
			
            var systemCardMembers = (await SystemCardMemberRepository.GetAsync());

			var items = (await Repository.GetAsync())
				.Where(x =>
					(x.LiquidationId == null) &&
                    (x.State == PaymentState.Active) &&
					(
						// Liquiaciones mias
						(x.Ticket.Concession.Concession.Supplier.Login == SessionData.Login) ||
						// Liquidaciones de las empresas de mi sistema de tarjetas
						(
							systemCardMembers
								.Any(y =>
									y.Login == x.Ticket.Concession.Concession.Supplier.Login &&
									y.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login
								)
						)
					) &&
					(x.Ticket.EventId == arguments.EventId)
				);
            if (arguments.Since != null)
                items = items
                    .Where(x =>
                        (x.Date >= arguments.Since)
                    );
            if (arguments.Until != null)
            {
                var until = arguments.Until?.AddDays(1);
                items = items
                    .Where(x =>
                        (x.Date < until)
                    );
            }
            if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.Ticket.Concession.Concession.Name.Contains(arguments.Filter)
					);

			var result = items
                .GroupBy(x => new { x.Ticket.Concession.Id, x.Ticket.Concession.Concession.Name })
				.Select(x => new
				{
					Amount = x.Sum(y => (Decimal?)y.Amount ?? 0),
					Payin = x.Sum(y => (Decimal?)y.Payin ?? 0),
					Since = x.Min(y => y.Date),
					Until = x.Max(y => y.Date),
					PaymentsCount = x.Count(),
					ConcessionId = x.Key.Id,
					ConcessionName = x.Key.Name
				})
				.ToList()
				.Select( x => new LiquidationGetCreateResult
				{
					Amount = x.Amount,
					Payin = x.Payin,
					Total = x.Amount - x.Payin,
					Since = x.Since.ToUTC(),
					Until = x.Until.ToUTC(),
					ConcessionName = x.ConcessionName
				});

			return new LiquidationGetCreateResultBase
				{
					eventName = (await EventRepository.GetAsync())
					.Where(
						x => x.Id == arguments.EventId
					).FirstOrDefault()?.Name ?? "",

					concessionName = (await PaymentConcessionRepository.GetAsync("Concession"))
					.Where(
						x => x.Id == arguments.ConcessionId
					)
					.FirstOrDefault()?.Concession?.Name ?? "",

					Data = result
				};
		}
		#endregion ExecuteAsync
	}
}
