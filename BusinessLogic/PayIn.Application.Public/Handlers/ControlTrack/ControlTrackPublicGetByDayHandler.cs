using PayIn.Application.Dto.Arguments.ControlTrack;
using PayIn.Application.Dto.Results.ControlTrack;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTrackPublicGetByDayHandler :
		IQueryBaseHandler<ControlTrackPublicGetByDayArguments, ControlTrackPublicGetByDayResult>
	{
		private readonly IEntityRepository<ControlTrack> Repository;

		#region Constructors
		public ControlTrackPublicGetByDayHandler(IEntityRepository<ControlTrack> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlTrackPublicGetByDayResult>> ExecuteAsync(ControlTrackPublicGetByDayArguments arguments)
		{
			var dateIni = arguments.Date.Value;
			var dateEnd = arguments.Date.Value.AddDays(1);

			var items = (await Repository.GetAsync())
				.Where(x =>
					(x.PresenceSince ?? x.PresenceUntil).Date >= dateIni &&
					(x.PresenceSince ?? x.PresenceUntil).Date < dateEnd
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
					},
					Items = x.Items
						.Select(y => new
						{
							Id = y.Id,
							Date = y.Date,
							Latitude = y.Latitude,
							Longitude = y.Longitude,
							Quality = (100 - y.Quality)
						})
				})
				.ToList()
				.Select(x => new ControlTrackPublicGetByDayResult
				{
					Id = x.Id,
					WorkerId = x.WorkerId,
					WorkerName = x.WorkerName,
					ItemId = x.ItemId,
					ItemName = x.ItemName,
					Since = x.Since == null ? null : new ControlTrackPublicGetByDayResult_Item
					{
						Id = x.Since.Id,
						Date = x.Since.Date,
						Latitude = x.Since.Latitude,
						Longitude = x.Since.Longitude
					},
					Until = x.Until == null ? null : new ControlTrackPublicGetByDayResult_Item
					{
						Id = x.Until.Id,
						Date = x.Until.Date,
						Latitude = x.Until.Latitude,
						Longitude = x.Until.Longitude
					},
					Items = x.Items
						.Select(y => new ControlTrackPublicGetByDayResult_Item
						{
							Id = y.Id,
							Date = y.Date,
							Latitude = y.Latitude,
							Longitude = y.Longitude,
							Quality = y.Quality
						})
				});

			foreach (var item in result)
				item.Items = PositionManager.Calculate(item);

			return new ResultBase<ControlTrackPublicGetByDayResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
