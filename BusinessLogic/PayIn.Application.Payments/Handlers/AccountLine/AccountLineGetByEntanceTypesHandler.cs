using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments
{
    public class AccountLineGetByEntranceTypesHandler :
		IQueryBaseHandler<AccountLineGetByEntranceTypesArguments, AccountLineGetByEntranceTypesResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<AccountLine> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<AccountLineGetByEntranceTypesResult>> ExecuteAsync(AccountLineGetByEntranceTypesArguments arguments)
		{
			if ((arguments.Since == null) && (arguments.Until == null))
				throw new ApplicationException("Debe especificar al menos una condición para el filtro");

            var now = DateTime.UtcNow;
			var since = arguments.Since ?? XpDate.MinValue;
			var until = arguments.Until?.AddDays(1) ?? XpDate.MaxValue;

            var items = (await Repository.GetAsync())
				.Where(x =>
                    (x.Ticket.State == TicketStateType.Active) &&
					(x.Ticket.Date >= since) &&
					(x.Ticket.Date < until) &&
					(x.Type == AccountLineType.Entrances) &&
					((arguments.Type == 0) || (x.Ticket.Lines.Any(y => y.Type == arguments.Type))) &&
					((arguments.EventId == null) || (x.Ticket.EventId == arguments.EventId)) &&
					((arguments.ServiceConcessionId == null) || (x.Concession.ConcessionId == arguments.ServiceConcessionId)) &&
					((arguments.EntranceType == null) || (x.Ticket.Lines.Any(y => y.EntranceTypeId == arguments.EntranceType))) &&
					(
						(SessionData.Roles.Contains(AccountRoles.Superadministrator)) ||
                        // Empresa liquidadora
                        (x.Ticket.LiquidationConcession.Concession.Supplier.Login == SessionData.Login) ||
                        (x.Ticket.LiquidationConcession.PaymentWorkers
                            .Any(y => y.Login == SessionData.Login)
                        )
					)
                )
                .Select(x => new
                {
                    TicketDate = x.Ticket.Date,
                    TicketLineType = x.Ticket.Lines
                        .Select(y => y.Type)
                        .FirstOrDefault(),
                    EventName = x.Ticket.Event.Name,
                    x.Id,
                    x.Amount,
				    ConcessionName = x.Concession.Concession.Name,
                    EntrancetTypeName = x.Ticket.Lines
                        .Where(y => y.Type == TicketLineType.Entrance)
                        .Select(y => y.EntranceType.Name)
                        .FirstOrDefault(),
                });
#if DEBUG
            var prefilter2 = items.ToList();
#endif // DEBUG

            var result = items
                .OrderByDescending(x => x.TicketDate)
                .ToList()
                .Select(x => new AccountLineGetByEntranceTypesResult
                {
                    Id = x.Id,
                    Type = x.TicketLineType,
                    TypeName = x.TicketLineType.ToEnumAlias(),
                    Date = x.TicketDate,
                    EventName = x.EventName,
                    Amount = x.Amount,
                    ConcessionName = x.ConcessionName,
                    EntranceTypeName = x.EntrancetTypeName
                })
#if DEBUG
                .ToList()
#endif
                ;

            return new AccountLineGetByEntranceTypesResultBase
            {
                Data = result,
                TotalRecharge = result
                    .Where(x => x.Type == TicketLineType.Recharge)
                    .Sum(x => (decimal?)x.Amount) ?? 0,
                TotalBuy = result
                    .Where(x => x.Type == TicketLineType.Buy)
                    .Sum(x => (decimal?)x.Amount) ?? 0,
                TotalReturnCard = result
                    .Where(x => x.Type == TicketLineType.ReturnCard)
                    .Sum(x => (decimal?)x.Amount) ?? 0,
                TotalDiscount = result
                    .Where(x => x.Type == TicketLineType.Discount)
                    .Sum(x => (decimal?)x.Amount) ?? 0,
                TotalProduct = result
                    .Where(x => x.Type == TicketLineType.Product)
                    .Sum(x => (decimal?)x.Amount) ?? 0,
                TotalEntrance = result
                    .Where(x => x.Type == TicketLineType.Entrance)
                    .Sum(x => (decimal?)x.Amount) ?? 0,
                TotalExtraPrice = result
                    .Where(x => x.Type == TicketLineType.ExtraPrice)
                    .Sum(x => (decimal?)x.Amount) ?? 0,
                Total = result
                    .Sum(x => (decimal?)x.Amount) ?? 0
            };
		}
        #endregion ExecuteAsync
    }
}
