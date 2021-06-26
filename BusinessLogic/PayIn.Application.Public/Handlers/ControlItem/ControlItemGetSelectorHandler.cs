using PayIn.Application.Dto.Arguments.ControlItem;
using PayIn.Application.Dto.Results.ControlItem;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlItemGetSelectorHandler :
		IQueryBaseHandler<ControlItemGetSelectorArguments, ControlItemGetSelectorResult>
	{
		private readonly IEntityRepository<ControlItem> _Repository;

		#region Constructors
		public ControlItemGetSelectorHandler(IEntityRepository<ControlItem> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ControlItemGetSelectorResult>> IQueryBaseHandler<ControlItemGetSelectorArguments, ControlItemGetSelectorResult>.ExecuteAsync(ControlItemGetSelectorArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Where(x => x.Name.Contains(arguments.Filter))
				.Select(x => new ControlItemGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<ControlItemGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
