using PayIn.Application.Dto.Arguments.ControlPlanning;
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
	public class ControlPlanningClearHandler :
		IServiceBaseHandler<ControlPlanningClearArguments>
	{
		private readonly IEntityRepository<ControlPlanning> Repository;

		#region Constructors
		public ControlPlanningClearHandler(
			IEntityRepository<ControlPlanning>     repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlPlanningClearArguments>.ExecuteAsync(ControlPlanningClearArguments arguments)
		{
			if (arguments.Since.Value > arguments.Until.Value)
				throw new XpException(ControlPlanningResources.SincePreviousUntilException);
			if (arguments.Since.Value.AddMonths(6) < DateTime.Now)
				throw new XpException(ControlPlanningResources.OldSinceException);
			if (DateTime.Now.AddMonths(12) < arguments.Until)
				throw new XpException(ControlPlanningResources.NewUntilException);

			var date = arguments.Since.Value.Date;
			while (date <= arguments.Until)
			{
				var items = (await Repository.GetAsync())
					.Where(x => 
						(x.WorkerId   == arguments.WorkerId) &&
						(x.ItemId     == arguments.ItemId) &&
						(x.Date.Year  == date.Year) &&
						(x.Date.Month == date.Month) &&
						(x.Date.Day   == date.Day)
					);
				foreach (var item in items)
					await Repository.DeleteAsync(item);

				date = date.AddDays(1);
			}

			return null;
		}
		#endregion ExecuteAsync
	}
}
