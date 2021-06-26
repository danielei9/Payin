using PayIn.Application.Dto.Arguments.ControlPlanningCheck;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPlanningCheckUpdateHandler :
		IServiceBaseHandler<ControlPlanningCheckUpdateArguments>
	{
		private readonly IEntityRepository<ControlPlanningCheck> Repository;

		#region Constructors
		public ControlPlanningCheckUpdateHandler(IEntityRepository<ControlPlanningCheck> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlPlanningCheckUpdateArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id, "Planning");

			item.Date = arguments.Date;

			return item;
		}
		#endregion ExecuteAsync
	}
}
