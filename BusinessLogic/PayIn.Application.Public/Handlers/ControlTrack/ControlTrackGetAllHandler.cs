using PayIn.Application.Dto.Arguments.ControlTrack;
using PayIn.Application.Dto.Results.ControlTrack;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTrackGetAllHandler :
		IQueryBaseHandler<ControlTrackGetAllArguments, ControlTrackGetAllResult>
	{
		private readonly IEntityRepository<ControlTrack> Repository;

		#region Constructors
		public ControlTrackGetAllHandler(IEntityRepository<ControlTrack> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlTrackGetAllResult>> ExecuteAsync(ControlTrackGetAllArguments arguments)
		{
			var date = arguments.Date;
			var nextDate = arguments.Date == null ? null: arguments.Date.AddDays(1);

			var items = (await Repository.GetAsync())
				.Where(x => (x.PresenceSince ?? x.PresenceUntil).Date >= date && (x.PresenceSince ?? x.PresenceUntil).Date < nextDate);
			if (arguments.WorkerId != null)
				items = items.Where(x => x.WorkerId == arguments.WorkerId);
			if (arguments.ItemId != null)
				items = items.Where(x => x.ItemId == arguments.ItemId);

			var result = items
				.OrderBy(x => new { (x.PresenceSince ?? x.PresenceUntil).Date })
				.Select(x => new
				{
					Id = x.Id,
					WorkerId = x.WorkerId,
					WorkerName = x.Worker.Name,
					ItemId = x.ItemId,
					ItemName = x.Item.Name,
					Since = x.PresenceSince == null ? null : new
					{
						Id = x.PresenceSince.Id,
						Date = x.PresenceSince.Date,
						Image = x.PresenceSince.Image,
						Latitude = x.PresenceSince.Latitude,
						Longitude = x.PresenceSince.Longitude,
						FormsCount = x.PresenceSince.PlanningCheck.FormAssigns.Count(),
						CheckId = x.PresenceSince.PlanningCheckId 
					},
					Until = x.PresenceUntil == null ? null : new
					{
						Id = x.PresenceUntil.Id,
						Date = x.PresenceUntil.Date,
						Image = x.PresenceUntil.Image,
						Latitude = x.PresenceUntil.Latitude,
						Longitude = x.PresenceUntil.Longitude,
						FormsCount = x.PresenceUntil.PlanningCheck.FormAssigns.Count(),
						CheckId = x.PresenceUntil.PlanningCheckId 
					}
				})
				.ToList()
				.Select(x => new ControlTrackGetAllResult
				{
					Id = x.Id,
					WorkerId = x.WorkerId,
					WorkerName = x.WorkerName,
					ItemId = x.ItemId,
					ItemName = x.ItemName,
					Since = x.Since == null ? null : new ControlTrackGetAllResult_Item
					{
						Id = x.Since.Id,
						Date = x.Since.Date,
						Image = x.Since.Image,
						Latitude = x.Since.Latitude,
						Longitude = x.Since.Longitude,
						FormsCount = x.Since.FormsCount,
						CheckId = x.Since.CheckId
					},
					Until = x.Until == null ? null : new ControlTrackGetAllResult_Item
					{
						Id = x.Until.Id,
						Date = x.Until.Date,
						Image = x.Until.Image,
						Latitude = x.Until.Latitude,
						Longitude = x.Until.Longitude,
						FormsCount = x.Until.FormsCount,
						CheckId = x.Until.CheckId
					}
				});

			return new ResultBase<ControlTrackGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
