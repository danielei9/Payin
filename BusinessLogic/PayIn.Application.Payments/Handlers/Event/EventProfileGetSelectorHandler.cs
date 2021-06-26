using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class EventProfileGetSelectorHandler :
		IQueryBaseHandler<EventProfileGetSelectorArguments, EventProfileGetSelectorResult>
	{
		private readonly IEntityRepository<Profile> Repository;

		#region Constructors
		public EventProfileGetSelectorHandler(IEntityRepository<Profile> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<EventProfileGetSelectorResult>> ExecuteAsync(EventProfileGetSelectorArguments arguments)
		{
			var items = (await Repository.GetAsync());

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.Name.Contains(arguments.Filter)
					);

			var result = items
				.Select(x => new EventProfileGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<EventProfileGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
