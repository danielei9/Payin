using PayIn.Application.Dto.Arguments.ControlIncident;
using PayIn.Application.Dto.Results.ControlIncident;
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
	/* Deprecated */
	public class ControlIncidentGetItemsHandler :
		IQueryBaseHandler<ControlIncidentGetItemsArguments, ControlIncidentGetItemsResult>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<ControlItem>       RepositoryControlItem;
		private readonly IEntityRepository<ServiceWorker>     RepositoryServiceWorker;
		private readonly IEntityRepository<ServiceConcession> RepositoryServiceConcession;
		private readonly IEntityRepository<ControlPresence>   RepositoryControlPresence;

		#region Constructors
		public ControlIncidentGetItemsHandler(
			ISessionData                         sessionData,
			IEntityRepository<ControlItem>       repositoryControlItem,
			IEntityRepository<ServiceWorker> repositoryServiceWorker,
			IEntityRepository<ServiceConcession> repositoryServiceConcession,
			IEntityRepository<ControlPresence>   repositoryControlPresence)
		{
			if (sessionData == null)                 throw new ArgumentNullException("sessionData");
			if (repositoryControlItem == null)       throw new ArgumentNullException("repositoryControlItem");
			if (repositoryServiceWorker == null)     throw new ArgumentNullException("repositoryServiceWorker");
			if (repositoryServiceConcession == null) throw new ArgumentNullException("repositoryServiceConcession");
			if (repositoryControlPresence == null)   throw new ArgumentNullException("repositoryControlPresence");

			SessionData                 = sessionData;
			RepositoryControlItem       = repositoryControlItem;
			RepositoryServiceWorker     = repositoryServiceWorker;
			RepositoryServiceConcession = repositoryServiceConcession;
			RepositoryControlPresence   = repositoryControlPresence;
		}
		#endregion Constructor
		
		#region ExecuteAsync
		async Task<ResultBase<ControlIncidentGetItemsResult>> IQueryBaseHandler<ControlIncidentGetItemsArguments, ControlIncidentGetItemsResult>.ExecuteAsync(ControlIncidentGetItemsArguments arguments)
		{
			var now = DateTime.Now;
			var yesterday = now.AddDays(-1);

			var supplierId = (await RepositoryServiceWorker.GetAsync())
				.Where(x => x.Login == SessionData.Login)
				.Select(x => x.SupplierId)
				.FirstOrDefault();

			var concessionIds = (await RepositoryServiceConcession.GetAsync())
				.Where(x => x.SupplierId == supplierId)
				.Select(x => x.Id)
				.ToList();

			var result = (await RepositoryControlItem.GetAsync())
				.Where(x => concessionIds.Contains(x.ConcessionId))
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					Observations = x.Observations,
					SaveTrack = x.SaveTrack,
					SaveFacialRecognition = x.SaveFacialRecognition,
					CheckTimetable = x.CheckTimetable,
					PresenceType = 
						x.Tracks
							.Where(y =>
								yesterday < (y.PresenceSince ?? y.PresenceUntil).Date &&
								(y.PresenceSince ?? y.PresenceUntil).Date <= now
							)
							//.OrderByDescending(y => (y.PresenceSince ?? y.PresenceUntil).Date)
							//.Take(1)
							.Any(y =>
								y.PresenceSince != null &&
								y.PresenceUntil == null
							) ? PresenceType.Exit : PresenceType.Entrance
				})
				.ToList()
				.Select(x => new ControlIncidentGetItemsResult
				{
					Id = x.Id,
					Name = x.Name,
					Observations = x.Observations,
					SaveTrack = x.SaveTrack,
					SaveFacialRecognition = x.SaveFacialRecognition,
					CheckTimetable = x.CheckTimetable,
					PresenceType = x.PresenceType
				})
				.ToList();

			return new ResultBase<ControlIncidentGetItemsResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
