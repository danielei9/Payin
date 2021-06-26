using PayIn.Application.Dto.Arguments.ControlPlanning;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPlanningUpdateHandler :
		IServiceBaseHandler<ControlPlanningUpdateArguments>
	{
		private readonly IEntityRepository<ControlPlanning> Repository;

		#region Constructors
		public ControlPlanningUpdateHandler(IEntityRepository<ControlPlanning> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlPlanningUpdateArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			item.CheckDuration = arguments.CheckDuration == null ? (XpDuration) null : arguments.CheckDuration;

			return item;
		}
		#endregion ExecuteAsync
	}
}
