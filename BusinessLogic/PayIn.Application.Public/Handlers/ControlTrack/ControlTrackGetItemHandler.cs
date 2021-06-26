using PayIn.Application.Dto.Arguments.ControlTrack;
using PayIn.Application.Dto.Results.ControlTrack;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTrackGetItemHandler :
		IQueryBaseHandler<ControlTrackGetItemArguments, ControlTrackGetItemResult>
	{
		private readonly IEntityRepository<ControlTrack> Repository;

		#region Constructors
		public ControlTrackGetItemHandler(
			IEntityRepository<ControlTrack> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlTrackGetItemResult>> ExecuteAsync(ControlTrackGetItemArguments arguments)
		{
			var date = arguments.Date.Value;
			var next = date.AddDays(1);

			var items = (await Repository.GetAsync());
			if (arguments.TrackId != null)
				items = items.Where(x => x.Id == arguments.TrackId);

			var result = items
				.Where(x =>
					x.ItemId == arguments.ItemId &&
					date <= (x.PresenceSince ?? x.PresenceUntil).Date && (x.PresenceSince ?? x.PresenceUntil).Date < next
				)
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
						.Where(y => y.Quality < 100)
						.OrderBy(y => y.Date)
						.Select(y => new {
							Id = y.Id,
							Date = y.Date,
							Longitude = y.Longitude,
							Latitude = y.Latitude,
							Quality = (100 - y.Quality)
						})
						.OrderBy(y => y.Date)
				})
				.ToList()
				.Select(x => new ControlTrackGetItemResult
				{
					Id = x.Id,
					WorkerId = x.WorkerId,
					WorkerName = x.WorkerName,
					ItemId = x.ItemId,
					ItemName = x.ItemName,
					Since = x.Since == null ? null : new ControlTrackGetItemResult_Item
					{
						Id = x.Since.Id,
						Date = x.Since.Date,
						Latitude = x.Since.Latitude.Value,
						Longitude = x.Since.Longitude.Value
					},
					Until = x.Until == null ? null : new ControlTrackGetItemResult_Item
					{
						Id = x.Until.Id,
						Date = x.Until.Date,
						Latitude = x.Until.Latitude.Value,
						Longitude = x.Until.Longitude.Value
					},
					Items = x.Items
						.Select(y => new ControlTrackGetItemResult_Item
						{
							Id = y.Id,
							Date = y.Date,
							Longitude = y.Longitude,
							Latitude = y.Latitude,
							Quality = y.Quality
						})
				})
				.ToList();

			foreach (var item in result)
				item.Items = PositionManager.Calculate(item);

			return new ResultBase<ControlTrackGetItemResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
