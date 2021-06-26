using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentGetChargesHandler:
		IQueryBaseHandler<PaymentGetChargesArguments, PaymentGetChargesResult>
	{
		[Dependency] public IEntityRepository<Domain.Payments.Payment> _Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		
		#region ExecuteAsync
		public async Task<ResultBase<PaymentGetChargesResult>> ExecuteAsync(PaymentGetChargesArguments arguments)
		{
			if (arguments.Since.Value > arguments.Until.Value)
				return new ResultBase<PaymentGetChargesResult>();

			var until = arguments.Until.AddDays(1);
			var items = (await _Repository.GetAsync())
				.Where(x=>
					x.Date >= arguments.Since &&
					x.Date < until &&
					(
					 x.Ticket.Concession.Concession.Supplier.Login == SessionData.Login ||
					 x.Ticket.PaymentWorker.Login == SessionData.Login || 
					 x.Ticket.LiquidationConcession.Concession.Supplier.Login == SessionData.Login
					 ) &&
					x.State == PaymentState.Active
				);
			
			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => 
					(x.Ticket.Reference.Contains(arguments.Filter) ||
					x.TaxName.Contains(arguments.Filter)
				));
				
			var result = items
				.Select(x => new
				{
					Id = x.Id,
					Ticket = x.Ticket.Reference,
					Amount = x.Amount,
					Payin = x.Payin,
					TicketAmount = x.Ticket.Amount,
					TaxName = x.TaxName,
					TicketId = x.TicketId,
					Date = x.Date,
					State = (x.RefundFromId != null && x.State == PaymentState.Active) ? PaymentState.Returned : x.State,
					RefundFromId = x.RefundFromId,
					RefundFromDate = (x.RefundFrom != null) ? (DateTime?)x.RefundFrom.Date : null,
					RefundToId = (x.RefundTo.Count() != 0) ? (int?)x.RefundTo.FirstOrDefault().Id : null,
					RefundToDate = (x.RefundTo.Count() != 0) ? (DateTime?)x.RefundTo.FirstOrDefault().Date : null
				})
				.OrderByDescending(x => x.Date)
				.ToList()
				.Select(x => new PaymentGetChargesResult
				{
					Id = x.Id,
					Ticket = x.Ticket,
					TaxName = x.TaxName,
					Amount = x.Amount,
					Payin = x.Payin,
					Total = x.Amount - x.Payin,
					TicketAmount = x.TicketAmount,
					TicketId = x.TicketId,
					Date = x.Date,
					State = x.State,
					RefundFromId = x.RefundFromId,
					RefundToId = x.RefundToId,
					RefundToDate = x.RefundToDate,
					RefundFromDate = x.RefundFromDate
				});

			return new ResultBase<PaymentGetChargesResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}