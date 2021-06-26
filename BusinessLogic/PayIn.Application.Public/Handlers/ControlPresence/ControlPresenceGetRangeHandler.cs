using PayIn.Application.Dto.Arguments.ControlPresence;
using PayIn.Application.Dto.Results.ControlPresence;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPresenceGetRangeHandler :
		IQueryBaseHandler<ControlPresenceGetRangeArguments, ControlPresenceGetRangeResult>
	{
		private readonly IEntityRepository<ControlPresence> Repository;

		#region Constructors
		public ControlPresenceGetRangeHandler(IEntityRepository<ControlPresence> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlPresenceGetRangeResult>> ExecuteAsync(ControlPresenceGetRangeArguments arguments)
		{
			var since = arguments.Since;
			var until = arguments.Until.AddDays(1);

			var presences = (await Repository.GetAsync("Item"))
				.Where(x =>
					x.WorkerId == arguments.WorkerId &&
					x.Date >= since &&
					x.Date < until
				)
				.OrderBy(x => x.Date);

			// Projecting presence regions
			var presencesRegion = new List<ControlPresenceGetRangeResult>();
			var sincePresences = new Dictionary<int, ControlPresence>();
			foreach (var presence in presences)
			{
				if (presence.PresenceType == PresenceType.Entrance)
				{
					if (sincePresences.ContainsKey(presence.ItemId))
					{
						var sincePresence = sincePresences[presence.ItemId];
						presencesRegion.Add(new ControlPresenceGetRangeResult
						{
							Title = presence.Item.Name,
							Info = presence.Item.Observations,
							CheckTimetable = presence.Item.CheckTimetable,
							Location = presence.Latitude + "," + presence.Longitude,
							StartId = sincePresence.Id,
							Start = sincePresence.Date,
							EndId = null,
							End = null,
							Type = CalendarItemResult.Types.Presence
						});
						sincePresences[presence.ItemId] = presence;
					}
					else 
						sincePresences.Add(presence.ItemId, presence);
				}
				else if (presence.PresenceType == PresenceType.Exit)
				{
					if (sincePresences.ContainsKey(presence.ItemId))
					{
						var sincePresence = sincePresences[presence.ItemId];
						presencesRegion.Add(new ControlPresenceGetRangeResult
						{
							Title = presence.Item.Name,
							Info = presence.Item.Observations,
							CheckTimetable = presence.Item.CheckTimetable,
							Location = presence.Latitude + "," + presence.Longitude,
							StartId = sincePresence.Id,
							Start = sincePresence.Date,
							EndId = presence.Id,
							End = presence.Date,
							Type = CalendarItemResult.Types.Presence
						});
						sincePresences.Remove(presence.ItemId);
					}
					else
					{
						presencesRegion.Add(new ControlPresenceGetRangeResult
						{
							Title = presence.Item.Name,
							Info = presence.Item.Observations,
							CheckTimetable = presence.Item.CheckTimetable,
							Location = presence.Latitude + "," + presence.Longitude,
							StartId = null,
							Start = null,
							EndId = presence.Id,
							End = presence.Date,
							Type = CalendarItemResult.Types.Presence
						});
					}
				}
			}
			foreach (var sincePresence in sincePresences.Values)
			{
				presencesRegion.Add(new ControlPresenceGetRangeResult
				{
					Title = sincePresence.Item.Name,
					Info = sincePresence.Item.Observations,
					CheckTimetable = sincePresence.Item.CheckTimetable,
					Location = sincePresence.Latitude + "," + sincePresence.Longitude,
					StartId = sincePresence.Id,
					Start = sincePresence.Date,
					EndId = null,
					End = null,
					Type = CalendarItemResult.Types.Presence
				});
			}

			return new ResultBase<ControlPresenceGetRangeResult> { Data = presencesRegion };
		}
		#endregion ExecuteAsync
	}
}
