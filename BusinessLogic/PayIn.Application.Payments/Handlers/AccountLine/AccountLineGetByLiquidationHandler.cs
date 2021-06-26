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
	public class AccountLineGetByLiquidationHandler :
		IQueryBaseHandler<AccountLineGetByLiquidationArguments, AccountLineGetByLiquidationResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<AccountLine> Repository { get; set; }
        [Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
        
        private string hexArray = "0123456789ABCDEF";

		#region ExecuteAsync
		public async Task<ResultBase<AccountLineGetByLiquidationResult>> ExecuteAsync(AccountLineGetByLiquidationArguments arguments)
		{
            //var serviceCards = await ServiceCardRepository.GetAsync();
            var myPaymentConcessions = (await PaymentConcessionRepository.GetAsync())
                .Where(x =>
                    (x.Concession.State == ConcessionState.Active) &&
                    (
                        (x.Concession.Supplier.Login == SessionData.Login) ||
                        (x.PaymentWorkers
                            .Any(y =>
                                (y.State == WorkerState.Active) &&
                                (y.Login == SessionData.Login)
                            )
                        )
                    )
                )
                .Select(x => x.Id)
                .ToList();

            var now = DateTime.UtcNow;

            var items = (await Repository.GetAsync())
                .Where(x =>
                    (x.Ticket.State == TicketStateType.Active) &&
					//(x.Type == AccountLineType.ServiceCard) &&
					(x.LiquidationId == arguments.LiquidationId) &&
                    (myPaymentConcessions
                        .Contains(x.Liquidation.LiquidationConcessionId)
                    )
                )
                .Select(x => new
                {
                    TicketDate = x.Ticket.Date,
                    TicketLineType = x.Ticket.Lines
                        .Select(y => y.Type)
                        .FirstOrDefault(),
                    EventName = x.Ticket.Event.Name,
					ConcessionName = x.Concession.Concession.Name,
                    x.Id,
                    x.Amount,
                    Liquidated = x.LiquidationId != null,
                    LiquidationState = x.Liquidation.State,
                    Paid = (bool?) (x.Liquidation.State == LiquidationState.Payed),
                    LineType = x.Type,
                    x.Uid,
                    UidText =
                       x.UidFormat == UidFormat.BigEndian ?
                           (
                               hexArray.Substring(((int)x.Uid / 268435456) % 16, 1) +
                               hexArray.Substring(((int)x.Uid / 16777216) % 16, 1) +
                               hexArray.Substring(((int)x.Uid / 1048576) % 16, 1) +
                               hexArray.Substring(((int)x.Uid / 65536) % 16, 1) +
                               hexArray.Substring(((int)x.Uid / 4096) % 16, 1) +
                               hexArray.Substring(((int)x.Uid / 256) % 16, 1) +
                               hexArray.Substring(((int)x.Uid / 16) % 16, 1) +
                               hexArray.Substring(((int)x.Uid / 1) % 16, 1)
                           ).ToString() :
                       x.UidFormat == UidFormat.LittleEndian ?
                           (
                               hexArray.Substring(((int)x.Uid / 16) % 16, 1) +
                               hexArray.Substring(((int)x.Uid / 1) % 16, 1) +
                               hexArray.Substring(((int)x.Uid / 4096) % 16, 1) +
                               hexArray.Substring(((int)x.Uid / 256) % 16, 1) +
                               hexArray.Substring(((int)x.Uid / 1048576) % 16, 1) +
                               hexArray.Substring(((int)x.Uid / 65536) % 16, 1) +
                               hexArray.Substring(((int)x.Uid / 268435456) % 16, 1) +
                               hexArray.Substring(((int)x.Uid / 16777216) % 16, 1)
                           ).ToString() :
                       x.Uid.ToString()
			    });
#if DEBUG
            var prefilter2 = items.ToList();
#endif // DEBUG

            var result = items
                .OrderByDescending(x => x.TicketDate)
                .ToList()
                .Select(x => new AccountLineGetByLiquidationResult
                {
                    Id = x.Id,
                    Type = x.TicketLineType,
                    TypeName = x.TicketLineType.ToEnumAlias(),
                    Date = x.TicketDate,
                    ConcessionName = x.ConcessionName,
                    EventName = x.EventName,
                    Amount = x.Amount,
                    Liquidated = x.Liquidated,
                    Paid = x.Paid ?? false,
                    LineType = x.LineType,
                    LineTypeName = x.LineType.ToEnumAlias(),
                    Uid = x.Uid.ToString() ?? "",
                    UidText = x.UidText,
                })
#if DEBUG
                .ToList()
#endif
                ;

            return new AccountLineGetByLiquidationResultBase
            {
                Data = result,
                Total = result
                    .Sum(x => (decimal?)x.Amount) ?? 0,
                State = items
                    .Select(x => x.LiquidationState)
                    .FirstOrDefault()
            };
		}
        #endregion ExecuteAsync
    }
}
