using PayIn.Application.Dto.Arguments.ControlPlanningCheck;
using PayIn.Application.Dto.Results.ControlPlanningCheck;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPlanningCheckMobileGetHandler :
		IQueryBaseHandler<ControlPlanningCheckMobileGetArguments, ControlPlanningCheckMobileGetResult>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<ControlPlanningCheck> RepositoryControlPlanningCheck;
		private readonly IEntityRepository<ControlItem> RepositoryControlItem;

		#region Constructors
		public ControlPlanningCheckMobileGetHandler(
			ISessionData sessionData,
			IEntityRepository<ControlPlanningCheck> repositoryControlPlanningCheck,
			IEntityRepository<ControlItem> repositoryControlItem)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repositoryControlPlanningCheck == null) throw new ArgumentNullException("repositoryControlPlanningCheck");
			if (repositoryControlItem == null) throw new ArgumentNullException("repositoryControlItem");

			SessionData = sessionData;
			RepositoryControlPlanningCheck = repositoryControlPlanningCheck;
			RepositoryControlItem = repositoryControlItem;
		}
		#endregion Constructor

		#region ExecuteAsync
		async Task<ResultBase<ControlPlanningCheckMobileGetResult>> IQueryBaseHandler<ControlPlanningCheckMobileGetArguments, ControlPlanningCheckMobileGetResult>.ExecuteAsync(ControlPlanningCheckMobileGetArguments arguments)
		{
			var now = DateTime.Now;
			var yesterday = now.AddDays(-1);
			var tomorrow = now.AddDays(1);

			var presenceType = (await RepositoryControlItem.GetAsync())
				.Where(x => x.Id == arguments.ItemId)
				.Select(x =>
					x.Tracks
						.Where(y =>
							yesterday < (y.PresenceSince ?? y.PresenceUntil).Date &&
							(y.PresenceSince ?? y.PresenceUntil).Date <= now
						)
						.OrderByDescending(y => (y.PresenceSince ?? y.PresenceUntil).Date)
						.Take(1)
						.Any(y =>
							y.PresenceSince != null &&
							y.PresenceUntil == null
						) ? PresenceType.Exit : PresenceType.Entrance
				)
				.FirstOrDefault();

			var planningCheck = (await RepositoryControlPlanningCheck.GetAsync());

			if (arguments.PlanningCheckId != null)
				planningCheck = planningCheck.Where(x => x.Id == arguments.PlanningCheckId);
			else if (presenceType == PresenceType.Entrance)
				planningCheck = planningCheck.Where(x => x.ItemsSince.Any(y => y.Id == arguments.PlanningItemId));
			else if (presenceType == PresenceType.Exit)
				planningCheck = planningCheck.Where(x => x.ItemsUntil.Any(y => y.Id == arguments.PlanningItemId));

			var result = planningCheck
				.Select(x => new ControlPlanningCheckMobileGetResult
				{
					Id = x.Planning.Item.Id,
					Name = x.Planning.Item.Name,
					Observations = x.Planning.Item.Observations,
					SaveTrack = x.Planning.Item.SaveTrack,
					SaveFacialRecognition = x.Planning.Item.SaveFacialRecognition,
					CheckTimetable = x.Planning.Item.CheckTimetable,
					PresenceType =
						x.Planning.Item.Tracks
							.Where(y =>
								yesterday < (y.PresenceSince ?? y.PresenceUntil).Date &&
								(y.PresenceSince ?? y.PresenceUntil).Date <= now
							)
						.OrderByDescending(y => (y.PresenceSince ?? y.PresenceUntil).Date)
						.Take(1)
						.Any(y =>
							y.PresenceSince != null &&
							y.PresenceUntil == null
						) ? PresenceType.Exit : PresenceType.Entrance,
				})
				.ToList();

			var check = planningCheck
				.Select(x => new
				{
					Id = x.Id,
					Date = x.Date,
					CheckId = x.Id,
					CheckPointId = x.CheckPointId,
					CheckPointType = x.CheckPoint != null ? x.CheckPoint.Type : CheckPointType.Check,
					Assigns = x.FormAssigns
						.Select(b => new
						{
							Id = b.Id,
							FormName = b.Form.Name,
							FormObservations = b.Form.Observations,
							Values = b.Values
							.Select(c => new
							{
								Id = c.Id,
								Name = c.Argument.Name,
								Type = c.Argument.Type,
								Target = c.Target,
								ValueString = c.ValueString,
								ValueNumeric = c.ValueNumeric,
								ValueBool = c.ValueBool,
								ValueDateTime = c.ValueDateTime
							})
						})
				})
				.ToList()
				.Select(x => new ControlPlanningCheckMobileGetResult_Planning
				{
					Id = x.Id,
					Date = x.Date,
					CheckId = x.CheckId,
					CheckPointId = x.CheckPointId,
					PresenceType = x.CheckPointType == CheckPointType.Round ? PresenceType.Round : presenceType,
					Assigns = x.Assigns
						.Select(y => new ControlPlanningCheckMobileGetResult_Assign
						{
							Id = y.Id,
							FormName = y.FormName,
							FormObservations = y.FormObservations,
							Values = y.Values
								.Select(z => new ControlPlanningCheckMobileGetResult_Value
								{
									Id = z.Id,
									Name = z.Name,
									Type = z.Type,
									Target = z.Target,
									ValueString = z.ValueString,
									ValueNumeric = z.ValueNumeric,
									ValueBool = z.ValueBool,
									ValueDateTime = (z.Type == ControlFormArgumentType.Datetime || z.Type == ControlFormArgumentType.Time)
										? ((z.ValueDateTime == null) ? null : z.ValueDateTime.ToUTC())
										: z.ValueDateTime
								})
						})
				})
				.ToList();

			result.FirstOrDefault().Plannings = check;

			return new ResultBase<ControlPlanningCheckMobileGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
