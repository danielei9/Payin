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

namespace PayIn.Application.Public.Handlers.Main
{
	public class TicketMobileGetDuesHandler : 
		IQueryBaseHandler<TicketMobileGetDuesArguments, TicketMobileGetDuesResult>
	{
		public ISessionData              SessionData { get; set; }
		public IEntityRepository<Ticket> Repository  { get; set; }

		#region Constructors
		public TicketMobileGetDuesHandler(
			ISessionData sessionData,
			IEntityRepository<Ticket> repository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			SessionData = sessionData;
			Repository = repository;
        }
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TicketMobileGetDuesResult>> ExecuteAsync(TicketMobileGetDuesArguments arguments)
		{
			var now = DateTime.Now;

			var tickets = (await Repository.GetAsync())
				.Where(x =>
					x.PaymentUser.Login == SessionData.Login &&
					now >= x.Since &&
					!(x.Payments.Any(z => z.State != PaymentState.Active) && x.Amount == 0)
				)
				.Select(x => new
				{
					Id = x.Id,
					Reference = x.Reference,
					Date = x.Date,
					Amount = x.Amount,
					PayedAmount = x.Payments
						.Where(y => y.State == PaymentState.Active)
						.Sum(y => (decimal?) y.Amount) ?? 0m,
					SupplierName = x.SupplierName,
					Until = x.Until,
					Since = x.Since,
				})
                .OrderByDescending(x => x.Date)
                .Skip(arguments.Skip)
				.Take(arguments.Top)
				.ToList()
				.Select(x => new TicketMobileGetDuesResult
				{
					Id = x.Id,
					Reference = x.Reference,
					Date = x.Date.ToUTC(),
					Amount = x.Amount,
					PayedAmount = x.PayedAmount,
					SupplierName = x.SupplierName,
					Until = x.Until.ToUTC(),
					Since = x.Since.ToUTC(),
				});

			return new ResultBase<TicketMobileGetDuesResult> {
				Data = tickets
			};
		}
		#endregion ExecuteAsync
	}
}
