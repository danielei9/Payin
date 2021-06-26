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
	public class ControlFormAssignTemplateGetCheckHandler :
		IQueryBaseHandler<ControlFormAssignTemplateGetCheckArguments, ControlFormAssignTemplateGetCheckResult>
	{
		private readonly IEntityRepository<ControlFormAssignTemplate> Repository;
		private readonly IEntityRepository<ControlTemplateCheck> RepositoryCheck;

		#region Constructors
		public ControlFormAssignTemplateGetCheckHandler(
			IEntityRepository<ControlTemplateCheck> repositoryCheck,
			IEntityRepository<ControlFormAssignTemplate> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryCheck == null) throw new ArgumentNullException("repositoryCheck");

			Repository = repository;
			RepositoryCheck = repositoryCheck;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlFormAssignTemplateGetCheckResult>> ExecuteAsync(ControlFormAssignTemplateGetCheckArguments arguments)
		{
			var item = (await RepositoryCheck.GetAsync())
				.Where(x => x.Id == arguments.CheckId)
				.Select(x => new {
					Time = x.Time
				})
				.FirstOrDefault();

			var items = await Repository.GetAsync();

			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => (
					x.Form.Name.Contains(arguments.Filter)
				));
			var result = items
				.Where(x => x.CheckId == arguments.CheckId)
				.OrderBy(x => new { x.Form.Name })
				.Select(x => new ControlFormAssignTemplateGetCheckResult
				{
					Id = x.Id,
					FormId = x.FormId,
					FormName = x.Form.Name,
					Arguments = x.Form.Arguments
					.Select(y => new ControlFormAssignTemplateGetCheckResult_Argument
					{
						Id = y.Id,
						Name = y.Name
					})
				});

			return new ControlFormAssignTemplateGetCheckResultBase
			{ 
				CheckId = arguments.CheckId,
				CheckTimeXT = item.Time,
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
