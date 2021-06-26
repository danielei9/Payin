using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments
{
	public class AccountLineGetByServiceCardsHandler :
		IQueryBaseHandler<AccountLineGetByServiceCardsArguments, AccountLineGetByServiceCardsResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<AccountLine> Repository { get; set; }
        [Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }
        
        private string hexArray = "0123456789ABCDEF";

		#region ExecuteAsync
		public async Task<ResultBase<AccountLineGetByServiceCardsResult>> ExecuteAsync(AccountLineGetByServiceCardsArguments arguments)
		{
			if ((arguments.Since == null) && (arguments.Until == null))
				throw new ApplicationException("Debe especificar al menos una condición para el filtro");

            var serviceCards = await ServiceCardRepository.GetAsync();

            var now = DateTime.UtcNow;
			var since = arguments.Since ?? XpDate.MinValue;
			var until = arguments.Until?.AddDays(1) ?? XpDate.MaxValue;

            var items = (await Repository.GetAsync())
                .Where(x =>
                    (x.Ticket.State == TicketStateType.Active) &&
                    (x.Ticket.Date >= since) &&
					(x.Ticket.Date < until) &&
					(x.Type == AccountLineType.ServiceCard) &&
					((arguments.Type == 0) || (x.Ticket.Lines.Any(y => y.Type == arguments.Type))) &&
					((arguments.EventId == null) || (x.Ticket.EventId == arguments.EventId)) &&
					((arguments.ServiceConcessionId == null) || (x.Concession.ConcessionId == arguments.ServiceConcessionId)) &&
					(
						(SessionData.Roles.Contains(AccountRoles.Superadministrator)) ||
                        // Empresa liquidadora
                        (x.Ticket.LiquidationConcession.Concession.Supplier.Login == SessionData.Login) ||
                        (x.Ticket.LiquidationConcession.PaymentWorkers
                            .Any(y => y.Login == SessionData.Login)
                        )
                    ) &&
                    (
                        (arguments.ServiceCardId == null) ||
                        (serviceCards
                            .Any(y =>
                                (y.Uid == x.Uid) &&
                                (y.Id == arguments.ServiceCardId)
                            )
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
                    ConcessionId = (int?)x.ConcessionId,
                    ConcessionName = x.Concession.Concession.Name,
                    x.Id,
                    x.Amount,
                    Liquidated = x.LiquidationId != null,
                    Paid = (bool?) (x.Liquidation.PaidQuantity > 0),
                    x.Uid,
                    UidText =
                        x.UidFormat == UidFormat.BigEndian ?
                            (
                                hexArray.Substring((int)((x.Uid / 268435456) % 16), 1) +
                                hexArray.Substring((int)((x.Uid / 16777216) % 16), 1) +
                                hexArray.Substring((int)((x.Uid / 1048576) % 16), 1) +
                                hexArray.Substring((int)((x.Uid / 65536) % 16), 1) +
                                hexArray.Substring((int)((x.Uid / 4096) % 16), 1) +
                                hexArray.Substring((int)((x.Uid / 256) % 16), 1) +
                                hexArray.Substring((int)((x.Uid / 16) % 16), 1) +
                                hexArray.Substring((int)((x.Uid / 1) % 16), 1)
                            ).ToString() :
                        x.UidFormat == UidFormat.LittleEndian ?
                            (
                                hexArray.Substring((int)((x.Uid / 16) % 16), 1) +
                                hexArray.Substring((int)((x.Uid / 1) % 16), 1) +
                                hexArray.Substring((int)((x.Uid / 4096) % 16), 1) +
                                hexArray.Substring((int)((x.Uid / 256) % 16), 1) +
                                hexArray.Substring((int)((x.Uid / 1048576) % 16), 1) +
                                hexArray.Substring((int)((x.Uid / 65536) % 16), 1) +
                                hexArray.Substring((int)((x.Uid / 268435456) % 16), 1) +
                                hexArray.Substring((int)((x.Uid / 16777216) % 16), 1)
                            ).ToString() :
                        x.Uid.ToString()
                });
#if DEBUG
            var prefilter2 = items.ToList();
#endif // DEBUG

            var result = items
                .OrderByDescending(x => x.TicketDate)
                .ToList()
                .Select(x => new AccountLineGetByServiceCardsResult
                {
                    Id = x.Id,
                    Type = x.TicketLineType,
                    TypeName = x.TicketLineType.ToEnumAlias(),
                    Date = x.TicketDate,
                    ConcessionId = x.ConcessionId,
                    ConcessionName = x.ConcessionName,
                    EventName = x.EventName,
                    Amount = x.Amount,
                    Liquidated = x.Liquidated,
                    Paid = x.Paid ?? false,
                    Uid = x.Uid.ToString() ?? "",
                    UidText = x.UidText
                })
#if DEBUG
                .ToList()
#endif
                ;

            return new AccountLineGetByServiceCardsResultBase
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
