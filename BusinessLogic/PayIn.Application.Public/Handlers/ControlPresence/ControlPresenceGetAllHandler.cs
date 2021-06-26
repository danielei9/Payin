using PayIn.Application.Dto.Arguments.ControlPresence;
using PayIn.Application.Dto.Results.ControlPresence;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPresenceGetAllHandler :
		IQueryBaseHandler<ControlPresenceGetAllArguments, ControlPresenceGetAllResult>
	{
		private readonly IEntityRepository<ControlPresence> Repository;

		#region Constructors
		public ControlPresenceGetAllHandler(IEntityRepository<ControlPresence> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ControlPresenceGetAllResult>> IQueryBaseHandler<ControlPresenceGetAllArguments, ControlPresenceGetAllResult>.ExecuteAsync(ControlPresenceGetAllArguments arguments)
		{
			var items = await Repository.GetAsync();

			var result = items
				.OrderBy(x => new { x.TagId })
				.Select(x => new
				{
					Id = x.Id,
					Date = x.Date,
					Latitude = x.Latitude,
					Longitude = x.Longitude,
					LatitudeWanted = x.LatitudeWanted,
					LongitudeWanted = x.LongitudeWanted,
					Observations = x.Observations,
					PresenceType = x.PresenceType,
					TagId = x.TagId,
					ItemId = x.ItemId
				})
				.ToList()
				.Select(x => new ControlPresenceGetAllResult
				{
					Id = x.Id,
					Date = x.Date, // Need to be done in memory
					Latitude = x.Latitude,
					Longitude = x.Longitude,
					LatitudeWanted = x.LatitudeWanted,
					LongitudeWanted = x.LongitudeWanted,
					Observations = x.Observations,
					PresenceType = x.PresenceType,
					TagId = x.TagId,
					ItemId = x.ItemId
				});

			return new ResultBase<ControlPresenceGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
