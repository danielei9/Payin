using PayIn.Application.Dto.Arguments.ControlPlanning;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Common.Exceptions;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPlanningCreateHandler :
		IServiceBaseHandler<ControlPlanningCreateArguments>
	{
		private readonly IEntityRepository<ControlPlanning>     Repository;
		private readonly IEntityRepository<ControlTemplate>     RepositoryTemplate;
		private readonly IEntityRepository<ControlPlanningItem> RepositoryPlanningItem;

		#region Constructors
		public ControlPlanningCreateHandler(
			IEntityRepository<ControlPlanning>     repository,
			IEntityRepository<ControlPlanningItem> repositoryPlanningItem,
			IEntityRepository<ControlTemplate>     repositoryTemplate
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryPlanningItem == null) throw new ArgumentNullException("repositoryPlanningItem");
			if (repositoryTemplate == null) throw new ArgumentNullException("repositoryTemplate");

			Repository = repository;
			RepositoryPlanningItem = repositoryPlanningItem;
			RepositoryTemplate = repositoryTemplate;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlPlanningCreateArguments>.ExecuteAsync(ControlPlanningCreateArguments arguments)
		{
			if (arguments.DateSince.Value > arguments.DateUntil.Value)
				throw new XpException(ControlPlanningResources.SincePreviousUntilException);
			if (arguments.DateSince.Value.AddMonths(6) < DateTime.Now)
				throw new XpException(ControlPlanningResources.OldSinceException);
			if (DateTime.Now.AddMonths(12) < arguments.DateUntil)
				throw new XpException(ControlPlanningResources.NewUntilException);

			var date = arguments.DateSince.Value.Date;
			while (date <= arguments.DateUntil)
			{
				var item = (await Repository.GetAsync("Items"))
					.Where(x => 
						(x.WorkerId   == arguments.WorkerId) &&
						(x.ItemId     == arguments.ItemId) &&
						(x.Date.Year  == date.Year) &&
						(x.Date.Month == date.Month) &&
						(x.Date.Day   == date.Day)
					).
					FirstOrDefault();
				if (item == null)
				{
					item = new ControlPlanning
					{
						CheckDuration = arguments.CheckDuration,
						Date = date,
						ItemId = arguments.ItemId,
						WorkerId = arguments.WorkerId
					};
					await Repository.AddAsync(item);
				}
				else
					item.CheckDuration = arguments.CheckDuration;

				date = date.AddDays(1);
			}

			return null;
		}
		#endregion ExecuteAsync
	}
}
