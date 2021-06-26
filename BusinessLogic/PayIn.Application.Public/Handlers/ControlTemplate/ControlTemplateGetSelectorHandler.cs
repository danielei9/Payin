using PayIn.Application.Dto.Arguments.ControlTemplate;
using PayIn.Application.Dto.Results.ControlTemplate;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTemplateGetSelectorHandler :
		IQueryBaseHandler<ControlTemplateGetSelectorArguments, ControlTemplateGetSelectorResult>
	{
		private readonly IEntityRepository<ControlTemplate> Repository;

		#region Constructors
		public ControlTemplateGetSelectorHandler(IEntityRepository<ControlTemplate> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ControlTemplateGetSelectorResult>> IQueryBaseHandler<ControlTemplateGetSelectorArguments, ControlTemplateGetSelectorResult>.ExecuteAsync(ControlTemplateGetSelectorArguments arguments)
		{
			var items = await Repository.GetAsync();
			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => x.Name.Contains(arguments.Filter));

			var result = items
				.Select(x => new ControlTemplateGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<ControlTemplateGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
