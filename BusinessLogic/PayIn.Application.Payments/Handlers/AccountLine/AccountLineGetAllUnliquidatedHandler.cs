using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using System;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments
{
	public class AccountLineGetAllUnliquidatedHandler :
		IQueryBaseHandler<AccountLineGetAllUnliquidatedArguments, AccountLineGetAllUnliquidatedResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<AccountLine> Repository { get; set; }
        //[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
        //[Dependency] public IEntityRepository<SystemCardMember> SystemCardMemberRepository { get; set; }

		private string hexArray = "0123456789ABCDEF";

		#region ExecuteAsync
		public async Task<ResultBase<AccountLineGetAllUnliquidatedResult>> ExecuteAsync(AccountLineGetAllUnliquidatedArguments arguments)
		{
			if ((arguments.Since == null) && (arguments.Until == null) && (!arguments.FilterByEvent) && (arguments.ConcessionId == null))
				throw new ApplicationException("Debe especificar al menos una condición para el filtro");

			var now = DateTime.UtcNow;
			var since = arguments.Since ?? XpDate.MinValue;
			var until = arguments.Until?.AddDays(1) ?? XpDate.MaxValue;

            //var systemCardMembers = (await SystemCardMemberRepository.GetAsync());
            //var paymentConcessions = (await PaymentConcessionRepository.GetAsync());

            var items = (await Repository.GetAsync())
				.Where(x =>
					//(
					//	(x.State == PaymentState.Active) ||
					//	(x.State == PaymentState.Returned)
					//) &&
					(x.Ticket.Date >= since) &&
					(x.Ticket.Date < until) &&
					//(x.Liquidation == null) &&
					(
						(SessionData.Roles.Contains(AccountRoles.Superadministrator)) ||
						(
                            (x.Ticket.LiquidationConcession.Concession.Supplier.Login == SessionData.Login) ||
                            (x.Ticket.LiquidationConcession.PaymentWorkers
                                .Any(y => y.Login == SessionData.Login)
                            )
                        )
					)
				);

			if (arguments.FilterByEvent)
			{
				items = items
					.Where(x =>
						(x.Ticket.EventId == arguments.EventId)
					);
			}
			if (arguments.ConcessionId != null)
			{
				items = items
					.Where(x =>
						x.Ticket.Concession.Id == arguments.ConcessionId
					);
			}

            var prefilter = items.Select(x => new
            {
                x.Id,
                x.Ticket.Date,
                x.Amount,
                Commission = 0, // x.Payin,
				TicketConcessionId = x.Ticket.ConcessionId,
				TicketConcessionName = x.Ticket.Concession.Concession.Name,
				x.Ticket.EventId,
				EventName = x.Ticket.Event.Name,
				Uid = x.Uid.ToString(),
				//UidText =
				//   x.UidFormat == UidFormat.BigEndian ?
				//	   (
				//		   hexArray.Substring(((int)x.Uid / 268435456) % 16, 1) +
				//		   hexArray.Substring(((int)x.Uid / 16777216) % 16, 1) +
				//		   hexArray.Substring(((int)x.Uid / 1048576) % 16, 1) +
				//		   hexArray.Substring(((int)x.Uid / 65536) % 16, 1) +
				//		   hexArray.Substring(((int)x.Uid / 4096) % 16, 1) +
				//		   hexArray.Substring(((int)x.Uid / 256) % 16, 1) +
				//		   hexArray.Substring(((int)x.Uid / 16) % 16, 1) +
				//		   hexArray.Substring(((int)x.Uid / 1) % 16, 1)
				//	   ).ToString() :
				//   x.UidFormat == UidFormat.LittleEndian ?
				//	   (
				//		   hexArray.Substring(((int)x.Uid / 16) % 16, 1) +
				//		   hexArray.Substring(((int)x.Uid / 1) % 16, 1) +
				//		   hexArray.Substring(((int)x.Uid / 4096) % 16, 1) +
				//		   hexArray.Substring(((int)x.Uid / 256) % 16, 1) +
				//		   hexArray.Substring(((int)x.Uid / 1048576) % 16, 1) +
				//		   hexArray.Substring(((int)x.Uid / 65536) % 16, 1) +
				//		   hexArray.Substring(((int)x.Uid / 268435456) % 16, 1) +
				//		   hexArray.Substring(((int)x.Uid / 16777216) % 16, 1)
				//	   ).ToString() :
				//   x.Uid.ToString(),
				//Seq = x.Seq.ToString(),
				x.Ticket,
				Title = x.Ticket.ServiceOperations.FirstOrDefault().Type, // .Lines.FirstOrDefault().Title,
				//x.State
			});
			var prefilter2 = prefilter.ToList();

			if (!(arguments.Filter == ""))
				prefilter = prefilter
					.Where(x =>
						x.Ticket.LiquidationConcession.Concession.Name.Contains(arguments.Filter) ||
						x.Title.Equals(arguments.Filter) ||
						x.Uid.Equals(arguments.Filter) //||
						//x.Seq.Equals(arguments.Filter) ||
						//x.UidText.Equals(arguments.Filter) ||
						//(x.Uid + " - " + x.Seq + "(" + x.UidText + ")").Equals(arguments.Filter)
				);
			prefilter2 = prefilter.ToList();

			var result = prefilter
				.OrderByDescending(x => x.Date)
				.ThenBy(x => x.TicketConcessionName)
				.ToList()
				.Select(x => new AccountLineGetAllUnliquidatedResult
				{
					Id = x.Id,
					Date = x.Date,
					Amount = x.Amount,
                    Commission = x.Commission,
                    TicketConcessionId = x.TicketConcessionId,
                    TicketConcessionName = x.TicketConcessionName,
					EventId = x.EventId,
					EventName = x.EventName,
					//Name =
     //                   x.Uid.IsNullOrEmpty() ? "" :
     //                   (
     //                       x.Uid +
     //                       (x.Seq.IsNullOrEmpty() ? "" : (" - " + x.Seq)) +
     //                       (x.UidText.IsNullOrEmpty() ? "" : (" (" + x.UidText + ")"))
     //                   ),
                    Title = x.Title.ToEnumAlias(),
					//State = x.State
				})
                .ToList();

            return new AccountLineGetAllUnliquidatedResultBase {
                Data = result,
                TotalAmount = Math.Round(result.Sum(x => (decimal?)x.Amount) ?? 0, 2),
                TotalCommission = Math.Round(result.Sum(x => x.Commission) ?? 0, 2)
            };
		}
        #endregion ExecuteAsync
    }
}
