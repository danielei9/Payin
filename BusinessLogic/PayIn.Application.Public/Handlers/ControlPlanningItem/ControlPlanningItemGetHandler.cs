using PayIn.Application.Dto.Arguments.ControlPlanningItem;
using PayIn.Application.Dto.Results.ControlPlanningItem;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPlanningItemGetHandler :
		IQueryBaseHandler<ControlPlanningItemGetArguments, ControlPlanningItemGetResult>
	{
		private readonly IEntityRepository<ControlPlanningItem> _Repository;

		#region Constructors
		public ControlPlanningItemGetHandler(IEntityRepository<ControlPlanningItem> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ControlPlanningItemGetResult>> IQueryBaseHandler<ControlPlanningItemGetArguments, ControlPlanningItemGetResult>.ExecuteAsync(ControlPlanningItemGetArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					Id              = x.Id,
					SinceId         = x.SinceId,
					Since           = x.Since,
					SinceFormsCount = x.Since.FormAssigns.Count,
					UntilId         = x.Until.Id,
					Until           = x.Until,
					UntilFormsCount = x.Until.FormAssigns.Count
				})
				.ToList()
				.Select(x => new ControlPlanningItemGetResult
				{
					Id              = x.Id,
					SinceId         = x.SinceId,
					Since           = x.Since.Date, // Need to be done in memory
					SinceFormsCount = x.SinceFormsCount,
					UntilId         = x.UntilId,
					Until           = x.Until.Date, // Need to be done in memory
					UntilFormsCount = x.UntilFormsCount
				});

			return new ResultBase<ControlPlanningItemGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
