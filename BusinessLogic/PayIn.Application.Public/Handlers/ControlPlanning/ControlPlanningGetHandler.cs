using PayIn.Application.Dto.Arguments.ControlPlanning;
using PayIn.Application.Dto.Results;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPlanningGetHandler :
		IQueryBaseHandler<ControlPlanningGetArguments, ControlPlanningGetResult>
	{
		private readonly IEntityRepository<ControlPlanning> _Repository;

		#region Constructors
		public ControlPlanningGetHandler(IEntityRepository<ControlPlanning> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlPlanningGetResult>> ExecuteAsync(ControlPlanningGetArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					Id = x.Id,
					CheckDuration = x.CheckDuration
				})
				.ToList()
				.Select(x => new ControlPlanningGetResult
				{
					Id = x.Id,
					CheckDuration = x.CheckDuration // Need to be done in memory
				});

			return new ResultBase<ControlPlanningGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
