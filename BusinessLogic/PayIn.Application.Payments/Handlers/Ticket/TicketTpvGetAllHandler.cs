using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class TicketTpvGetAllHandler : 
		IQueryBaseHandler<TicketTpvGetAllArguments, TicketTpvGetAllResult>
	{
		public ISessionData SessionData { get; set; }
		public IEntityRepository<Ticket> TicketRepository { get; set; }

		#region Constructors
		public TicketTpvGetAllHandler(
			ISessionData sessionData,
			IEntityRepository<Ticket> ticketRepository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (ticketRepository == null) throw new ArgumentNullException("ticketRepository");
			SessionData = sessionData;
			TicketRepository = ticketRepository;
        }
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TicketTpvGetAllResult>> ExecuteAsync(TicketTpvGetAllArguments arguments)
		{
			var nowUTC = DateTime.Now.ToUTC();

			var items = (await TicketRepository.GetAsync());

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.Id.ToString() == arguments.Filter ||
						x.Reference == arguments.Filter ||
						x.SupplierName == arguments.Filter
                    );

			var result = items
				.Where(x =>
					(
						x.Concession.Concession.Supplier.Login == SessionData.Login ||
						x.PaymentWorker.Login == SessionData.Login ||
						//x.Payments.Any(y => y.UserLogin == SessionData.Login) ||
						(x.Payments.Count() == 0 && (x.Concession.PaymentWorkers.Any(y => y.Login == SessionData.Login)))
					) &&
					x.Since < nowUTC &&
					x.Until >= nowUTC &&
					!(x.Payments.Any(z => z.State != PaymentState.Active) && x.Amount == 0))
				.Select(x => new
				{
					Id = x.Id,
					Reference = x.Reference,
					Date = x.Date,
					Amount = x.Amount,
					PayedAmount = x.Payments
						.Where(y => y.State == PaymentState.Active)
						.Sum(y => (decimal?)y.Amount) ?? 0m,
					SupplierName = x.SupplierName,
				})
				.OrderByDescending(x => x.Date)
				.ToList()
				.Select(x => new TicketTpvGetAllResult
				{
					Id = x.Id,
					Reference = x.Reference,
					Date = x.Date.ToUTC(),
					Amount = x.Amount,
					PayedAmount = x.PayedAmount,
					SupplierName = x.SupplierName,
				});

			return new ResultBase<TicketTpvGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
