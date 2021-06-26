using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class TicketGetHandler :
		IQueryBaseHandler<TicketGetArguments, TicketGetResult>
	{
		private readonly IEntityRepository<Ticket> _Repository;

		#region Constructors
		public TicketGetHandler(
			IEntityRepository<PayIn.Domain.Payments.Ticket> repository,
			IEntityRepository<PayIn.Domain.Payments.PaymentConcession> concessionRepository
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TicketGetResult>> ExecuteAsync(TicketGetArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					Id = x.Id,
					Reference = x.Reference,
					//Title = x.Title,
					Amount = x.Amount,
					SupplierName = x.SupplierName,
					State = x.State,
					Date = x.Date,
					Since = x.Since,
					Until = x.Until
				})
				.ToList()
				.Select(x => new TicketGetResult
				{
					Id = x.Id,
					Reference = x.Reference,
					//Title = x.Title,
					Amount = x.Amount,
					SupplierName = x.SupplierName,
					State = x.State,
					Date = x.Date.ToUTC(), // It's needed to calculate in memory
					Since = x.Since.ToUTC(),
					Until = x.Until.ToUTC()
				});

			return new ResultBase<TicketGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
