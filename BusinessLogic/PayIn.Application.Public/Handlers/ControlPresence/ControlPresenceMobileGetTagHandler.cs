using PayIn.Application.Dto.Arguments.ControlPresence;
using PayIn.Application.Dto.Results.ControlPresence;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPresenceMobileGetTagHandler :
		IQueryBaseHandler<ControlPresenceMobileGetTagArguments, ControlPresenceMobileGetTagResult>
	{
		private readonly IEntityRepository<ServiceTag> Repository;
		private readonly IEntityRepository<ControlPresence>   RepositoryControlPresence;

		#region Constructors
		public ControlPresenceMobileGetTagHandler(
			IEntityRepository<ServiceTag> repository,
			IEntityRepository<ControlPresence> repositoryControlPresence)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryControlPresence == null) throw new ArgumentNullException("repositoryControlPresence");

			Repository = repository;
			RepositoryControlPresence = repositoryControlPresence;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ControlPresenceMobileGetTagResult>> IQueryBaseHandler<ControlPresenceMobileGetTagArguments, ControlPresenceMobileGetTagResult>.ExecuteAsync(ControlPresenceMobileGetTagArguments arguments)
		{
			var now = DateTime.Now;
			var yesterday = now.AddDays(-1);
			var items = await Repository.GetAsync();

			var result = items
				.Where(x => 
					(x.Reference == arguments.Reference)
				)
				.Select(x => new ControlPresenceMobileGetTagResult {
					Id        = x.Id,
					Reference = x.Reference,
					Items     = x.CheckPoints
						.Select(y => new ControlPresenceMobileGetTagResult_Item {
							Id                    = y.ItemId,
							Name                  = y.Item.Name,
							Observations          = y.Item.Observations,
							SaveFacialRecognition = y.Item.SaveFacialRecognition,
							SaveTrack             = y.Item.SaveTrack,
							PresenceType          = 
								y.Type == CheckPointType.Entrance ? PresenceType.Entrance :
								y.Type == CheckPointType.Exit     ? PresenceType.Exit :
								y.Item.Tracks
									.Where(z =>
										yesterday < (z.PresenceSince ?? z.PresenceUntil).Date &&
										(z.PresenceSince ?? z.PresenceUntil).Date <= now
									)
									.OrderByDescending(z => (z.PresenceSince ?? z.PresenceUntil).Date)
									.Take(1)
									.Any(z =>
										z.PresenceSince != null &&
										z.PresenceUntil == null
									) ? PresenceType.Exit : PresenceType.Entrance
						})
				});

			return new ResultBase<ControlPresenceMobileGetTagResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
