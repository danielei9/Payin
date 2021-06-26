using PayIn.Application.Dto.Arguments.ControlPlanningItem;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPlanningItemUpdateHandler :
		IServiceBaseHandler<ControlPlanningItemUpdateArguments>
	{
		private readonly IEntityRepository<ControlPlanningItem> Repository;
		private readonly IEntityRepository<ControlPlanningCheck> PlanningCheckRepository;

		#region Constructors
		public ControlPlanningItemUpdateHandler(IEntityRepository<ControlPlanningItem> repository, IEntityRepository<ControlPlanningCheck> planningCheckRepository)
		{
			if (repository == null)              throw new ArgumentNullException("repository");
			if (planningCheckRepository == null) throw new ArgumentNullException("planningCheckRepository");

			Repository              = repository;
			PlanningCheckRepository = planningCheckRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlPlanningItemUpdateArguments>.ExecuteAsync(ControlPlanningItemUpdateArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id, "Planning", "Since", "Until");
			var planningDate = new DateTime(item.Planning.Date.Year, item.Planning.Date.Month, item.Planning.Date.Day, 0, 0, 0, DateTimeKind.Local);

			item.Since.Date = planningDate.Add(arguments.Since.Value.Value).ToUTC();
			item.Until.Date = arguments.Until.Value < arguments.Since.Value ?
					planningDate.AddDays(1).Add(arguments.Until.Value.Value).ToUTC() :
					planningDate.Add(arguments.Until.Value.Value).ToUTC();

			return item;
		}
		#endregion ExecuteAsync
	}
}
