using PayIn.Application.Dto.Arguments.ControlPlanningItem;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPlanningItemDeleteHandler :
			IServiceBaseHandler<ControlPlanningItemDeleteArguments>
	{
		private readonly IEntityRepository<ControlPlanningItem> Repository;
		private readonly IEntityRepository<ControlPlanningCheck> PlanningCheckRepository;

		#region Constructors
		public ControlPlanningItemDeleteHandler(IEntityRepository<ControlPlanningItem> repository, IEntityRepository<ControlPlanningCheck> planningCheckRepository)
		{
			if (repository == null)              throw new ArgumentNullException("repository");
			if (planningCheckRepository == null) throw new ArgumentNullException("planningCheckRepository"); 
			
			Repository              = repository;
			PlanningCheckRepository = planningCheckRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlPlanningItemDeleteArguments>.ExecuteAsync(ControlPlanningItemDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id, "Since", "Until");

			item.Since.ItemsSince.Remove(item);

			if (item.Since.ItemsSince.Count == 0)
				await PlanningCheckRepository.DeleteAsync(item.Since);

			item.Until.ItemsUntil.Remove(item);

			if (item.Until.ItemsUntil.Count == 0)
				await PlanningCheckRepository.DeleteAsync(item.Until);

			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
