using PayIn.Application.Dto.Arguments.ControlIncident;
using PayIn.Application.Dto.Results.ControlIncident;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Application.Public.Handlers
{
	public class ControlIncidentCreateManualCheckHandler :
		IServiceBaseHandler<ControlIncidentCreateManualCheckArguments>
	{
		private readonly IUnitOfWork UnitOfWork;
		private readonly ISessionData SessionData;
		private readonly IBlobRepository BlobRepository;
		private readonly IEntityRepository<ControlIncident> Repository;
		private readonly IEntityRepository<ControlItem> RepositoryControlItem;
		private readonly IEntityRepository<ControlPresence> RepositoryControlPresence;
		private readonly IEntityRepository<ControlTrack> RepositoryControlTrack;
		private readonly IEntityRepository<ServiceCheckPoint> RepositoryServiceCheckPoint;
		private readonly IEntityRepository<ServiceWorker> RepositoryServiceWorker;
		private readonly IEntityRepository<ControlFormValue> RepositoryFormValue;

		#region Constructors
		public ControlIncidentCreateManualCheckHandler(
			IUnitOfWork unitOfWork,
			ISessionData sessionData,
			IBlobRepository blobRepository,
			IEntityRepository<ControlIncident> repository,
			IEntityRepository<ControlItem> repositoryControlItem,
			IEntityRepository<ControlPresence> repositoryControlPresence,
			IEntityRepository<ControlTrack> repositoryControlTrack,
			IEntityRepository<ServiceCheckPoint> repositoryServiceCheckPoint,
			IEntityRepository<ServiceWorker> repositoryServiceWorker,
			IEntityRepository<ControlFormValue> repositoryFormValue
		)
		{
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (blobRepository == null) throw new ArgumentNullException("blobRepository");
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryControlItem == null) throw new ArgumentNullException("repositoryControlItem");
			if (repositoryControlPresence == null) throw new ArgumentNullException("repositoryControlPresence");
			if (repositoryControlTrack == null) throw new ArgumentNullException("repositoryControlTrack");
			if (repositoryServiceCheckPoint == null) throw new ArgumentNullException("repositoryServiceCheckPoint");
			if (repositoryServiceWorker == null) throw new ArgumentNullException("repositoryServceWorker");
			if (repositoryFormValue == null) throw new ArgumentNullException("repositoryFormValue");

			UnitOfWork = unitOfWork;
			SessionData = sessionData;
			BlobRepository = blobRepository;
			Repository = repository;
			RepositoryControlItem = repositoryControlItem;
			RepositoryControlPresence = repositoryControlPresence;
			RepositoryControlTrack = repositoryControlTrack;
			RepositoryServiceCheckPoint = repositoryServiceCheckPoint;
			RepositoryServiceWorker = repositoryServiceWorker;
			RepositoryFormValue = repositoryFormValue;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlIncidentCreateManualCheckArguments arguments)
		{
			var now = DateTime.Now;
			var date = arguments.Item.Date;
			var yesterday = date.AddDays(-1);
			var entrances = new List<ControlTrack>();
			var exits = new List<ControlTrack>();
			var trackFrecuency = DateTime.MaxValue;

			if (arguments.IncidentType != IncidentType.ManualCheck)
				throw new ArgumentOutOfRangeException("Incorrect type");

			var worker = (await RepositoryServiceWorker.GetAsync())
				.Where(x => x.Login == SessionData.Login)
				.FirstOrDefault();
			if (worker == null)
				throw new ArgumentNullException("sessiondata.login");

			var item = await RepositoryControlItem.GetAsync(arguments.Item.Id);
			if (item == null)
				throw new ArgumentNullException("Items.Id");

			var track = (await RepositoryControlTrack.GetAsync("PresenceUntil"))
				.Where(x =>
					x.Worker.Login == SessionData.Login &&
					x.ItemId == arguments.Item.Id &&
					x.PresenceSince != null &&
					yesterday < x.PresenceSince.Date && x.PresenceSince.Date <= date
				)
				.OrderByDescending(x => x.PresenceSince.Date)
				.ThenByDescending(x => x.PresenceSince.CreatedAt)
				.FirstOrDefault();

			var type = CheckPointType.Entrance;
			if (arguments.Item.CheckPointId != null)
			{
				var checkPoint = await RepositoryServiceCheckPoint.GetAsync((int)arguments.Item.CheckPointId);
				if (checkPoint == null)
					throw new ArgumentNullException("Items.CheckId");
				if (checkPoint.Type == CheckPointType.Round)
					type = CheckPointType.Round;
			}
			if (type != CheckPointType.Round)
			{
				if (((track == null) || (track.PresenceUntil != null)))
					type = CheckPointType.Entrance;
				else
					type = CheckPointType.Exit;
			}

			if ((type == CheckPointType.Entrance) || (type == CheckPointType.Round) || (track == null) || (track.PresenceUntil != null))
			{
				track = new ControlTrack
				{
					CreatedAt = now,
					ItemId = arguments.Item.Id,
					WorkerId = worker.Id
				};
				await RepositoryControlTrack.AddAsync(track);
			}

			if (item.SaveTrack && (type != CheckPointType.Round))
			{
				if (type == CheckPointType.Entrance)
				{
					var trackFrec = new DateTime(1900, 1, 1, 0, item.TrackFrecuency.Hour, item.TrackFrecuency.Minute);

					entrances.Add(track);
					trackFrecuency = trackFrec;
				}
				else
					exits.Add(track);
			}

			if (arguments.Item.CheckId != null)
			{
				if (arguments.Item.Assigns != null)
					foreach (var AssignArguments in arguments.Item.Assigns)
					{
						foreach (var ValueArguments in AssignArguments.Values)
						{
							var Value = await RepositoryFormValue.GetAsync(ValueArguments.Id);
							if (Value == null)
								throw new ArgumentNullException("Value.Id");
							if (Value.Target == ControlFormArgumentTarget.Check)
							{
								if (Value.Argument.Type == ControlFormArgumentType.Bool)
									Value.ValueBool = ValueArguments.ValueBool;
								else if (Value.Argument.Type == ControlFormArgumentType.Int || Value.Argument.Type == ControlFormArgumentType.Double)
									Value.ValueNumeric = ValueArguments.ValueNumeric;
								else if (Value.Argument.Type == ControlFormArgumentType.String)
									Value.ValueString = ValueArguments.ValueString;
								else if (Value.Argument.Type == ControlFormArgumentType.Datetime || Value.Argument.Type == ControlFormArgumentType.Time)
									Value.ValueDateTime = ValueArguments.ValueDateTime.ToUTC();
								else if (Value.Argument.Type == ControlFormArgumentType.Duration || Value.Argument.Type == ControlFormArgumentType.Date)
									Value.ValueDateTime = ValueArguments.ValueDateTime;
							}
						}
					}
			}

			var presence = new ControlPresence
			{
				CreatedAt = now,
				Date = date,
				Latitude = arguments.Item.Latitude,
				LatitudeWanted = arguments.Item.Latitude,
				Longitude = arguments.Item.Longitude,
				LongitudeWanted = arguments.Item.Longitude,
				Observations = arguments.Observations ?? "",
				CheckPointId = arguments.Item.CheckPointId,
				PlanningCheckId = arguments.Item.CheckId,
				TrackSince = (type == CheckPointType.Entrance || type == CheckPointType.Round) ? track : null,
				TrackUntil = (type == CheckPointType.Exit || type == CheckPointType.Round) ? track : null
			};
			await RepositoryControlPresence.AddAsync(presence);

			var incident = new ControlIncident
			{
				Type = arguments.IncidentType,
				CreatedAt = now,
				Observations = arguments.Observations,
				Source = "ControlPresence",
				SourceId = presence.Id
			};
			await Repository.AddAsync(incident);

			await UnitOfWork.SaveAsync();

			// Save image
			if (item.SaveFacialRecognition)
				presence.Image = BlobRepository.SaveFile(ControlPresenceResources.FotoShortUrl.FormatString(presence.Id), arguments.Item.Image);

			var result = new ControlIncidentCreateManualCheckResult
			{
				Entrances = entrances.Select(x => new ControlIncidentCreateManualCheckResult.Track { Id = x.Id }),
				Exits = exits.Select(x => new ControlIncidentCreateManualCheckResult.Track { Id = x.Id }),
				TrackFrecuency = trackFrecuency
			};
			return result;
		}
		#endregion ExecuteAsync
	}
}
