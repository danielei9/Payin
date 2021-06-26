using PayIn.Application.Dto.Arguments.ControlTrack;
using PayIn.Application.Dto.Results.ControlTrack;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTrackGetHandler :
		IQueryBaseHandler<ControlTrackGetArguments, ControlTrackGetResult>
	{
		private readonly IEntityRepository<ControlTrack> Repository;

		#region Constructors
		public ControlTrackGetHandler(
			IEntityRepository<ControlTrack> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlTrackGetResult>> ExecuteAsync(ControlTrackGetArguments arguments)
		{
			var items = (await Repository.GetAsync());		
			
			var result = items
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					Id = x.Id,
					WorkerId = x.WorkerId,
					WorkerName = x.Worker.Name,
					ItemId = x.ItemId,
					ItemName = x.Item.Name,
					Since =  x.PresenceSince == null ? null : new {
						Id = x.PresenceSince.Id,
						Date = x.PresenceSince.Date,
						Latitude = x.PresenceSince.Latitude,
						Longitude = x.PresenceSince.Longitude
					},
					Until = x.PresenceUntil == null ? null : new {
						Id = x.PresenceUntil.Id,
						Date = x.PresenceUntil.Date,
						Latitude = x.PresenceUntil.Latitude,
						Longitude = x.PresenceUntil.Longitude
					},
					Items = x.Items
						.Where(y =>
							(y.Quality < 200 &&
							y.Date <= arguments.End &&
							y.Date >= arguments.Start)
							 // (y.Quality < 200)
						)
						.Select(y => new {
							Id = y.Id,
							Date = y.Date,
							Longitude = y.Longitude,
							Latitude = y.Latitude,
							Quality = (200 - y.Quality) * 100,
							MobileVelocity = y.Speed
						})
						.OrderBy(y => y.Date)
				})
				.ToList()
				.Select(x => new ControlTrackGetResult
				{
					Id = x.Id,
					WorkerId = x.WorkerId,
					WorkerName = x.WorkerName,
					ItemId = x.ItemId,
					ItemName = x.ItemName,
					Since = x.Since == null ? null : new ControlTrackGetResult_Item {
						Id = x.Since.Id,
						Date = x.Since.Date,
						Latitude = x.Since.Latitude,
						Longitude = x.Since.Longitude
					},
					Until = x.Until == null ? null : new ControlTrackGetResult_Item
					{
						Id = x.Until.Id,
						Date = x.Until.Date,
						Latitude = x.Until.Latitude,
						Longitude = x.Until.Longitude
					},
					Items = x.Items
					.Select(y => new ControlTrackGetResult_Item
					{
						Id = y.Id,
						Date = y.Date,
						Longitude = y.Longitude,
						Latitude = y.Latitude,
						Quality = y.Quality,
						MobileVelocity = (decimal) y.MobileVelocity,
						Acceleration = 0,
						Distance = 0,
						Velocity = 0
					})
				})
				.ToList();

			foreach (var item in result)
				item.Items = PositionManager.Calculate(item);

			return new ResultBase<ControlTrackGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
