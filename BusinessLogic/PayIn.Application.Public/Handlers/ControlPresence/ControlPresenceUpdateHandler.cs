using PayIn.Application.Dto.Arguments.ControlPresence;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPresenceUpdateHandler :
		IServiceBaseHandler<ControlPresenceUpdateArguments>
	{
		private readonly IEntityRepository<ControlPresence> _Repository;

		#region Constructors
		public ControlPresenceUpdateHandler(IEntityRepository<ControlPresence> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlPresenceUpdateArguments>.ExecuteAsync(ControlPresenceUpdateArguments arguments)
		{
			var item = await _Repository.GetAsync(arguments.Id);
			item.Date = arguments.Date;
			item.Latitude = arguments.Latitude;
			item.Longitude = arguments.Longitude;
			item.LatitudeWanted = arguments.LatitudeWanted;
			item.LongitudeWanted = arguments.LongitudeWanted;
			item.Observations = arguments.Observations;
			item.PresenceType = arguments.PresenceType;

			return item;
		}
		#endregion ExecuteAsync
	}
}
