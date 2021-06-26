using PayIn.Application.Dto.Arguments.ControlFormAssignTemplate;
using PayIn.Application.Dto.Results.ControlFormAssignTemplate;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormAssignTemplateGetHandler :
		IQueryBaseHandler<ControlFormAssignTemplateGetArguments, ControlFormAssignTemplateGetResult>
	{
		private readonly IEntityRepository<ControlFormAssignTemplate> Repository;

		#region Constructors
		public ControlFormAssignTemplateGetHandler(
			IEntityRepository<ControlFormAssignTemplate> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlFormAssignTemplateGetResult>> ExecuteAsync(ControlFormAssignTemplateGetArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.OrderBy(x => new { x.Form.Name })
				.Select(x => new ControlFormAssignTemplateGetResult
				{
					Id = x.Id,
					Arguments = x.Form.Arguments
					.Select(y => new ControlFormAssignTemplateGetResult_Argument
					{

						Id = y.Id,
						Name = y.Name,
						Observations = y.Observations,
						MinOptions = y.MinOptions,
						MaxOptions = y.MaxOptions,
						Target = y.Target,
						Type = y.Type
					})
				});

			return new ResultBase<ControlFormAssignTemplateGetResult> { Data = items };
		}
		#endregion ExecuteAsync
	}
}
