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
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentsGetAllHandler :
		IQueryBaseHandler<PaymentGetAllArguments, PaymentsGetAllResult>
	{
		[Dependency] public IEntityRepository<Payment> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

        private string hexArray = "0123456789ABCDEF";
		
		#region ExecuteAsync
		public async Task<ResultBase<PaymentsGetAllResult>> ExecuteAsync(PaymentGetAllArguments arguments)
		{
			if (arguments.Since.Value > arguments.Until.Value)
				return new ResultBase<PaymentsGetAllResult>();

			var until = arguments.Until.AddDays(1);
			var items = (await Repository.GetAsync())
				.Where(x=>
					x.Date >= arguments.Since && 
					x.Date < until &&
					(x.UserLogin == SessionData.Login || SessionData.Roles.Contains(AccountRoles.Superadministrator))
				)
                .Select(x => new
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Payin = x.Payin,
                    TicketId = x.TicketId,
                    TicketAmount = x.Ticket.Amount,
                    Uid = x.Uid,
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
                        x.Uid.ToString(),
                    Seq = x.Seq,
                    TaxName = x.TaxName,
                    Date = x.Date,
                    State = (x.RefundFromId != null && x.State == PaymentState.Active) ? PaymentState.Returned : x.State,
                    RefundFromId = x.RefundFromId,
                    RefundFromDate = (x.RefundFrom != null) ? (DateTime?)x.RefundFrom.Date : null,
                    RefundToId = (x.RefundTo.Count() != 0) ? (int?)x.RefundTo.FirstOrDefault().Id : null,
                    RefundToDate = (x.RefundTo.Count() != 0) ? (DateTime?)x.RefundTo.FirstOrDefault().Date : null
                });
            
            if (!arguments.Filter.IsNullOrEmpty())
                items = items.Where(x => (
                    x.Uid.ToString().Contains(arguments.Filter) ||
                    x.UidText.Contains(arguments.Filter) ||
                    x.TaxName.Contains(arguments.Filter)
                ));

            var result = items
				.OrderByDescending(x => x.Date)
				.ToList()
				.Select(x => new PaymentsGetAllResult
				{
					Id = x.Id,
                    Uid = x.Uid,
                    UidText = x.UidText ?? "",
                    Seq = x.Seq,
                    TaxName = x.TaxName,
					Amount = x.Amount,
					Payin = x.Payin,
                    TicketId = x.TicketId,
					TicketAmount = x.TicketAmount,
					Date = x.Date.ToUTC(),
					State = x.State,
					RefundFromId = x.RefundFromId,
					RefundToId = x.RefundToId,
					RefundToDate = x.RefundToDate,
					RefundFromDate = x.RefundFromDate
				});

			return new ResultBase<PaymentsGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
