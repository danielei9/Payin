using PayIn.Application.Dto.Arguments.ControlPlanningCheck;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common.Exceptions;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPlanningCheckCreateHandler :
		IServiceBaseHandler<ControlPlanningCheckCreateArguments>
	{
		private readonly IEntityRepository<ControlPlanning> RepositoryControlPlanning;
		private readonly IEntityRepository<ServiceCheckPoint> RepositoryServiceCheckPoint;

		#region Constructors
		public ControlPlanningCheckCreateHandler(
			IEntityRepository<ControlPlanning> repositoryControlPlanning,
			IEntityRepository<ServiceCheckPoint> repositoryServiceCheckPoint
		)
		{
			if (repositoryControlPlanning == null) throw new ArgumentNullException("repositoryControlPlanning");
			if (repositoryServiceCheckPoint == null) throw new ArgumentNullException("repositoryServiceCheckPoint");
			RepositoryControlPlanning = repositoryControlPlanning;
			RepositoryServiceCheckPoint = repositoryServiceCheckPoint;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlPlanningCheckCreateArguments arguments)
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
				var item = (await RepositoryControlPlanning.GetAsync("Items"))
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
						CheckDuration = null,
						Date = date,
						ItemId = arguments.ItemId,
						WorkerId = arguments.WorkerId
					};
					await RepositoryControlPlanning.AddAsync(item);
				}

				var checkpoint = await RepositoryServiceCheckPoint.GetAsync(arguments.CheckPointId);
				if (checkpoint.Type != Common.CheckPointType.Round)
					throw new ApplicationException(ControlPlanningCheckResources.OnlyTypeRound);

				var planningDate = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, 0, 0, 0, DateTimeKind.Local);
				item.Checks.Add(new ControlPlanningCheck
				{
					Date = planningDate.Add(arguments.Time.Value.Value).ToUTC(),
					CheckPointId = arguments.CheckPointId
				});

				date = date.AddDays(1);
			}

			return null;
		}
		#endregion ExecuteAsync
	}
}
