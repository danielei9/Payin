using PayIn.Application.Dto.Arguments.ControlPresence;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPresenceCreateHandler : 
		IServiceBaseHandler<ControlPresenceCreateArguments>	
	{
		private readonly IEntityRepository<ControlPresence> _Repository;

		#region Constructors
		public ControlPresenceCreateHandler(IEntityRepository<ControlPresence> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlPresenceCreateArguments>.ExecuteAsync(ControlPresenceCreateArguments arguments)
		{
			var itemControlPresence = new ControlPresence
			{
				Date = arguments.Date,
				Latitude = arguments.Latitude,
				Longitude = arguments.Longitude,
				LatitudeWanted = arguments.LatitudeWanted,
				LongitudeWanted = arguments.LongitudeWanted,
				Observations = arguments.Observations,
				PresenceType = arguments.PresenceType,
				TagId = arguments.TagId,
				ItemId = arguments.ItemId
			};
			await _Repository.AddAsync(itemControlPresence);

			return itemControlPresence;
		}
		#endregion ExecuteAsync
	}
}
