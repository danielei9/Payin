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
	public class EventGetVisibilityHandler :
		IQueryBaseHandler<EventGetVisibilityArguments, EventGetVisibilityResult>
	{
		private readonly IEntityRepository<Event> Repository;
		
		#region Constructors
		public EventGetVisibilityHandler(
			IEntityRepository<Event> repository,
			IEntityRepository<EntranceSystem> entranceSystemRepository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<EventGetVisibilityResult>> ExecuteAsync(EventGetVisibilityArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id);

			var result = item
				.Select(x => new EventGetVisibilityResult
				{
					Id = x.Id,
					Visibility = x.Visibility,
				 });

			return new ResultBase<EventGetVisibilityResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
