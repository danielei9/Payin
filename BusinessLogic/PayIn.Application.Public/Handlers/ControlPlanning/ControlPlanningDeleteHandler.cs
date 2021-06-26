using PayIn.Application.Dto.Arguments.ControlPlanning;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPlanningDeleteHandler :
			IServiceBaseHandler<ControlPlanningDeleteArguments>
	{
		private readonly IEntityRepository<ControlPlanning> Repository;
		private readonly IEntityRepository<ControlPlanningCheck> RepositoryControlPlanningCheck;

		#region Constructors
		public ControlPlanningDeleteHandler(
			IEntityRepository<ControlPlanning> repository,
			IEntityRepository<ControlPlanningCheck> repositoryControlPlanningCheck
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryControlPlanningCheck == null) throw new ArgumentNullException("repositoryControlPlanningCheck");
			Repository = repository;
			RepositoryControlPlanningCheck = repositoryControlPlanningCheck;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlPlanningDeleteArguments>.ExecuteAsync(ControlPlanningDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
