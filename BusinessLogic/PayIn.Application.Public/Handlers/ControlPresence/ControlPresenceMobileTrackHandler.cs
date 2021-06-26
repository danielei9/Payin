using PayIn.Application.Dto.Arguments.ControlPresence;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPresenceMobileTrackHandler :
		IServiceBaseHandler<ControlPresenceMobileTrackArguments>
	{
		private readonly ISessionData                        SessionData;
		private readonly IEntityRepository<ControlTrackItem> Repository;

		#region Constructors
		public ControlPresenceMobileTrackHandler(
			ISessionData                        sessionData,
			IEntityRepository<ControlTrackItem> repository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null)  throw new ArgumentNullException("repository");
			
			SessionData             = sessionData;
			Repository              = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlPresenceMobileTrackArguments>.ExecuteAsync(ControlPresenceMobileTrackArguments arguments)
		{
			var now = DateTime.Now;

			foreach (var trackArgument in arguments.Tracks)
			{
				foreach (var itemArgument in arguments.TrackItems)
				{
					var item = new ControlTrackItem
					{
						CreatedAt = now,
						Date = itemArgument.Date,
						TrackId = trackArgument.Id,
						Latitude = itemArgument.Latitude,
						Longitude = itemArgument.Longitude,
						Quality = itemArgument.Quality,
						Speed = itemArgument.Speed
					};
					await Repository.AddAsync(item);
				}
			}

			return null;
		}
		#endregion ExecuteAsync
	}
}
