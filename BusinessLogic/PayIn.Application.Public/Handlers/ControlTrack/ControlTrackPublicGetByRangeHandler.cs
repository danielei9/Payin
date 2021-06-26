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
	public class ControlTrackPublicGetByRangeHandler :
		IQueryBaseHandler<ControlTrackPublicGetByRangeArguments, ControlTrackPublicGetByRangeResult>
	{
		private readonly IEntityRepository<ControlTrack> Repository;

		#region Constructors
		public ControlTrackPublicGetByRangeHandler(IEntityRepository<ControlTrack> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlTrackPublicGetByRangeResult>> ExecuteAsync(ControlTrackPublicGetByRangeArguments arguments)
		{
			var dateIni = arguments.Since.Value;
			var dateEnd = arguments.Until.Value;

			var items = (await Repository.GetAsync())
				.Where(x =>
					(x.PresenceSince ?? x.PresenceUntil).Date >= dateIni &&
					(x.PresenceSince ?? x.PresenceUntil).Date <= dateEnd
				);
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
						Latitude = x.PresenceSince.Latitude,
						Longitude = x.PresenceSince.Longitude
					},
					Until = x.PresenceUntil == null ? null : new
					{
						Id = x.PresenceUntil.Id,
						Date = x.PresenceUntil.Date,
						Latitude = x.PresenceUntil.Latitude,
						Longitude = x.PresenceUntil.Longitude
					}
				})
				.ToList()
				.Select(x => new ControlTrackPublicGetByRangeResult
				{
					Id = x.Id,
					WorkerId = x.WorkerId,
					WorkerName = x.WorkerName,
					ItemId = x.ItemId,
					ItemName = x.ItemName,
					Since = x.Since == null ? null : new ControlTrackPublicGetByRangeResult_Item
					{
						Id = x.Since.Id,
						Date = x.Since.Date,
						Latitude = x.Since.Latitude,
						Longitude = x.Since.Longitude
					},
					Until = x.Until == null ? null : new ControlTrackPublicGetByRangeResult_Item
					{
						Id = x.Until.Id,
						Date = x.Until.Date,
						Latitude = x.Until.Latitude,
						Longitude = x.Until.Longitude
					}
				});

			return new ResultBase<ControlTrackPublicGetByRangeResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
