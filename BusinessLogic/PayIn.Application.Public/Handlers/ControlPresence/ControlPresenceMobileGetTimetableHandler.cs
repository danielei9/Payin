using PayIn.Application.Dto.Arguments.ControlPresence;
using PayIn.Application.Dto.Results.ControlPresence;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using serV = PayIn.Domain.Public.ServiceWorker;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;
using PayIn.Common;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPresenceGetTimetableHandler :
		IQueryBaseHandler<ControlPresenceMobileGetTimetableArguments, ControlPresenceMobileGetTimetableResult>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<ControlPlanning> RepositoryControlPlanning;
		private readonly IEntityRepository<ControlTrack> RepositoryControlTrack;

		#region Constructors
		public ControlPresenceGetTimetableHandler(
			ISessionData sessionData,
			IEntityRepository<ControlPlanning> repositoryControlPlanning,
			IEntityRepository<ControlTrack> repositoryControlTrack
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repositoryControlPlanning == null) throw new ArgumentNullException("repositoryControlPlanning");
			if (repositoryControlTrack == null) throw new ArgumentNullException("repositoryControlTrack");

			SessionData = sessionData;
			RepositoryControlPlanning = repositoryControlPlanning;
			RepositoryControlTrack = repositoryControlTrack;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ControlPresenceMobileGetTimetableResult>> IQueryBaseHandler<ControlPresenceMobileGetTimetableArguments, ControlPresenceMobileGetTimetableResult>.ExecuteAsync(ControlPresenceMobileGetTimetableArguments arguments)
		{
			var tomorrow = arguments.Until.AddDays(1);

			var presences = (await RepositoryControlTrack.GetAsync())
				.Where(x =>
					(x.Worker.Login == SessionData.Login) &&
					(x.PresenceSince ?? x.PresenceUntil).Date >= arguments.Since &&
					(x.PresenceSince ?? x.PresenceUntil).Date < tomorrow
				)
				.Select(x => new
				{
					Id = x.Id,
					Title = x.Item.Name,
					Info = x.Item.Observations,
					Location =
						x.PresenceSince != null ? x.PresenceSince.Latitude + "," + x.PresenceSince.Longitude :
						x.PresenceUntil != null ? x.PresenceUntil.Latitude + "," + x.PresenceUntil.Longitude :
						"",
					Start = x.PresenceSince == null ? (DateTime?)null : x.PresenceSince.Date,
					End = x.PresenceUntil == null ? (DateTime?)null : x.PresenceUntil.Date,
					ItemId = x.ItemId
				})
				.ToList()
				.Select(x => new ControlPresenceMobileGetTimetableResult
				{
					Id = x.Id,
					Title = x.Title,
					Info = x.Info,
					Location = x.Location,
					Start = x.Start, // Need to be done in memory
					End = x.End, // Need to be done in memory
					Type = CalendarItemResult.Types.Presence,
					ItemId = x.ItemId
				});

			var controlPlannings = (await RepositoryControlPlanning.GetAsync())
				.Where(x =>
					x.Worker.Login == SessionData.Login &&
					x.Date >= arguments.Since &&
					x.Date < tomorrow
				)
				.SelectMany(x => (x.Items
						.Select(y => new
						{
							Id = y.Id,
							Title = x.Item.Name,
							Info = x.Item.Observations,
							Start = y.Since,
							End = y.Until,
							ItemId = x.ItemId,
							Type = CalendarItemResult.Types.Planning,
							Location = ""
						})
					).Union(x.Checks
						.Where(y => y.ItemsSince.Count() + y.ItemsUntil.Count() == 0)
						.Select(y => new
						{
							Id = y.Id,
							Title = x.Item.Name,
							Info = x.Item.Observations,
							Start = y,
							End = y,
							ItemId = x.ItemId,
							Type = CalendarItemResult.Types.Check,
							Location = y.CheckPoint == null ? "" : y.CheckPoint.Latitude + "," + y.CheckPoint.Longitude
						})
					)
				)
				.ToList()
				.Select(x => new ControlPresenceMobileGetTimetableResult
				{
					Id = x.Id,
					Title = x.Title,
					Info = x.Info,
					Location = x.Location,
					Start = x.Start.Date, // Need to be done in memory
					End = x.End.Date, // Need to be done in memory
					Type = x.Type,
					ItemId = x.ItemId
				})
				.Union(presences)
				.OrderBy(x => (x.Start ?? x.End).Value);

			return new ResultBase<ControlPresenceMobileGetTimetableResult> { Data = controlPlannings };
		}
		#endregion ExecuteAsync
	}
}
