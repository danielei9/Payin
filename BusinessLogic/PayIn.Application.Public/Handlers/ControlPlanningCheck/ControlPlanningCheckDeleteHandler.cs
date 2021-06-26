using PayIn.Application.Dto.Arguments.ControlPlanningCheck;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPlanningCheckDeleteHandler :
			IServiceBaseHandler<ControlPlanningCheckDeleteArguments>
	{
		private readonly IEntityRepository<ControlPlanningCheck> Repository;

		#region Constructors
		public ControlPlanningCheckDeleteHandler(IEntityRepository<ControlPlanningCheck> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlPlanningCheckDeleteArguments>.ExecuteAsync(ControlPlanningCheckDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
