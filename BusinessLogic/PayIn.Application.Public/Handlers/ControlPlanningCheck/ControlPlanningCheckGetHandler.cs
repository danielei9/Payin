using PayIn.Application.Dto.Arguments.ControlPlanningCheck;
using PayIn.Application.Dto.Results.ControlPlanningCheck;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPlanningCheckGetHandler :
		IQueryBaseHandler<ControlPlanningCheckGetArguments, ControlPlanningCheckGetResult>
	{
		private readonly IEntityRepository<ControlPlanningCheck> _Repository;

		#region Constructors
		public ControlPlanningCheckGetHandler(IEntityRepository<ControlPlanningCheck> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlPlanningCheckGetResult>> ExecuteAsync(ControlPlanningCheckGetArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Where(x => x.Id == arguments.Id)
				.Select(x => new ControlPlanningCheckGetResult
				{
					Id = x.Id,
					DateDT = x.Date,
					FormsCount = x.FormAssigns.Count(),
					WorkerId = x.Planning.WorkerId,
					WorkerName =x.Planning.Worker.Name
				});

			return new ResultBase<ControlPlanningCheckGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
