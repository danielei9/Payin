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
	public class ControlTemplateItemGetTemplateHandler :
		IQueryBaseHandler<ControlTemplateItemGetTemplateArguments, ControlTemplateItemGetTemplateResult>
	{
		private readonly IEntityRepository<ControlTemplateItem> _Repository;

		#region Constructors
		public ControlTemplateItemGetTemplateHandler(IEntityRepository<ControlTemplateItem> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ControlTemplateItemGetTemplateResult>> IQueryBaseHandler<ControlTemplateItemGetTemplateArguments, ControlTemplateItemGetTemplateResult>.ExecuteAsync(ControlTemplateItemGetTemplateArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Where(x => x.TemplateId == arguments.TemplateId)
				.Select(x => new
				{
					Id = x.Id,
					Since = x.Since.Time,
					Until = x.Until.Time,
					SinceFormsCount = x.Since.FormAssignTemplates.Count(),
					UntilFormsCount = x.Until.FormAssignTemplates.Count(),
					SinceId =   x.SinceId,
					UntilId = x.UntilId
				})
				.ToList()
				.Select(x => new ControlTemplateItemGetTemplateResult
				{
					Id = x.Id,
					Since = x.Since, // Need to be done in memory
					Until = x.Until, // Need to be done in memory
					SinceFormsCount = x.SinceFormsCount,
					UntilFormsCount = x.UntilFormsCount,
					SinceId = x.SinceId,
					UntilId = x.UntilId
				});

			return new ResultBase<ControlTemplateItemGetTemplateResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
