using PayIn.Application.Dto.Arguments.ControlPlanningItem;
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
	public class ControlPlanningItemCreateHandler :
		IServiceBaseHandler<ControlPlanningItemCreateArguments>
	{
		private readonly IEntityRepository<ControlPlanning>     RepositoryControlPlanning;

		#region Constructors
		public ControlPlanningItemCreateHandler(
			IEntityRepository<ControlPlanning> repositoryControlPlanning
		)
		{
			if (repositoryControlPlanning == null) throw new ArgumentNullException("repositoryControlPlanning");
			RepositoryControlPlanning = repositoryControlPlanning;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlPlanningItemCreateArguments>.ExecuteAsync(ControlPlanningItemCreateArguments arguments)
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

				var planningDate = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, 0, 0, 0, DateTimeKind.Local);
				item.Items.Add(new ControlPlanningItem
				{
					Since = new ControlPlanningCheck {
						Date = planningDate.Add(arguments.TimeSince.Value.Value).ToUTC(),
						Planning = item
					},
					Until = new ControlPlanningCheck {
						Date = arguments.TimeUntil.Value < arguments.TimeSince.Value ?
							planningDate.AddDays(1).Add(arguments.TimeUntil.Value.Value).ToUTC() :
							planningDate.Add(arguments.TimeUntil.Value.Value).ToUTC(),
						Planning = item
					}
				});

				date = date.AddDays(1);
			}

			return null;
		}
		#endregion ExecuteAsync
	}
}
