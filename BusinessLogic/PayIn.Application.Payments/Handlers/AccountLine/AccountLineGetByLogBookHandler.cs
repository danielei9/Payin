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
	public class AccountLineGetByLogBookHandler :
		IQueryBaseHandler<AccountLineGetByLogBookArguments, AccountLineGetByLogBookResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<AccountLine> Repository { get; set; }
        
        private string hexArray = "0123456789ABCDEF";

		#region ExecuteAsync
		public async Task<ResultBase<AccountLineGetByLogBookResult>> ExecuteAsync(AccountLineGetByLogBookArguments arguments)
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
                    TicketId = x.Ticket.Id,
                    TicketDate = x.Ticket.Date,
                    TicketLineType = x.Ticket.Lines
                        .Select(y => y.Type)
                        .FirstOrDefault(),
                    TicketEventName = x.Ticket.Event.Name,
                    TicketConcessionName = x.Ticket.Concession.Concession.Name,
                    TicketLiquidationConcessionName = x.Ticket.LiquidationConcession.Concession.Name,
                    x.Id,
                    x.Amount,
                    Liquidated = x.LiquidationId != null,
                    Paid = (bool?)(x.Liquidation.PaidQuantity > 0),
                    ConcessionName = x.Concession.Concession.Name,
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
                       x.Uid.ToString(),
                    x.Ticket,
				    x.Type,
			    });
#if DEBUG
            var prefilter2 = items.ToList();
#endif

            var result = items
                .GroupBy(x => new { x.TicketId, x.TicketDate, x.TicketLineType, x.TicketConcessionName, x.TicketLiquidationConcessionName, x.TicketEventName })
                .OrderByDescending(x => x.Key.TicketDate)
                .ToList()
                .Select(x => new AccountLineGetByLogBookResult
                {
                    Id = x.Key.TicketId,
                    TypeName = x.Key.TicketLineType.ToEnumAlias(),
                    Date = x.Key.TicketDate,
                    EventName = x.Key.TicketEventName,
                    ConcessionName = x.Key.TicketConcessionName,
                    LiquidationConcessionName = x.Key.TicketLiquidationConcessionName,
                    TotalDebit = x
                        .Where(y => y.Amount > 0)
                        .Sum(y => (decimal?)y.Amount) ?? 0,
                    TotalCredit = -1 * x
                        .Where(y => y.Amount < 0)
                        .Sum(y => (decimal?)y.Amount) ?? 0,
                    Lines = x
                        .Select(y => new AccountLineGetByLogBookResultLine
                        {
                            Id = y.Id,
                            Amount = y.Amount,
                            Liquidated = y.Liquidated,
                            Paid = y.Paid ?? false,
                            ConcessionName = y.ConcessionName,
                            Uid = y.Uid.ToString() ?? "",
                            UidText = y.UidText,
                            Type = y.Type,
                            TypeName = y.Type.ToEnumAlias()
                        })
                })
#if DEBUG
                .ToList()
#endif
                ;

            return new AccountLineGetByLogBookResultBase
            {
                TotalCash = result
                    .Sum(x => x.Lines
                        .Where(y => y.Type == AccountLineType.Cash)
                        .Sum(y => (decimal?)y.Amount)
                    ) ?? 0,
                TotalServiceCard = result
                    .Sum(x => x.Lines
                        .Where(y => y.Type == AccountLineType.ServiceCard)
                        .Sum(y => (decimal?)y.Amount)
                    ) ?? 0,
                TotalCreditCard = result
                    .Sum(x => x.Lines
                        .Where(y => y.Type == AccountLineType.CreditCard)
                        .Sum(y => (decimal?)y.Amount)
                    ) ?? 0,
                TotalProducts = result
                    .Sum(x => x.Lines
                        .Where(y => y.Type == AccountLineType.Products)
                        .Sum(y => (decimal?)y.Amount)
                    ) ?? 0,
                TotalEntrances = result
                    .Sum(x => x.Lines
                        .Where(y => y.Type == AccountLineType.Entrances)
                        .Sum(y => (decimal?)y.Amount)
                    ) ?? 0,
                TotalOthers = result
                    .Sum(x => x.Lines
                        .Where(y => y.Type == AccountLineType.Others)
                        .Sum(y => (decimal?)y.Amount)
                    ) ?? 0,
                Data = result
            };
		}
        #endregion ExecuteAsync
    }
}
