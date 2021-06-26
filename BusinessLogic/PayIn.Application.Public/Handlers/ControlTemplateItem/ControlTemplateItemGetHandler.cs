using PayIn.Application.Dto.Arguments.ControlTemplateItem;
using PayIn.Application.Dto.Results.ControlTemplateItem;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTemplateItemGetHandler :
		IQueryBaseHandler<ControlTemplateItemGetArguments, ControlTemplateItemGetResult>
	{
		private readonly IEntityRepository<ControlTemplateItem> _Repository;

		#region Constructors
		public ControlTemplateItemGetHandler(IEntityRepository<ControlTemplateItem> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ControlTemplateItemGetResult>> IQueryBaseHandler<ControlTemplateItemGetArguments, ControlTemplateItemGetResult>.ExecuteAsync(ControlTemplateItemGetArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					Id    = x.Id,
					Since = x.Since.Time,
					Until = x.Until.Time
				})
				.ToList()
				.Select(x => new ControlTemplateItemGetResult
				{
					Id = x.Id,
					Since = x.Since, // Need to be done in memory
					Until = x.Until // Need to be done in memory
				});

			return new ResultBase<ControlTemplateItemGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
