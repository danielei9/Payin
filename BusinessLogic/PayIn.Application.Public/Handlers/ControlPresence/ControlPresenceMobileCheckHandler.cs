using PayIn.Application.Dto.Arguments.ControlPresence;
using PayIn.Application.Dto.Results.ControlPresence;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPresenceMobileCheckHandler :
		IServiceBaseHandler<ControlPresenceMobileCheckArguments>
	{
		private readonly IUnitOfWork UnitOfWork;
		private readonly ISessionData SessionData;
		private readonly IBlobRepository BlobRepository;
		private readonly IEntityRepository<ControlPresence> Repository;
		private readonly IEntityRepository<ServiceTag> RepositoryServiceTag;
		private readonly IEntityRepository<ControlItem> RepositoryControlItem;
		private readonly IEntityRepository<ControlTrack> RepositoryControlTrack;
		private readonly IEntityRepository<ServiceCheckPoint> RepositoryServiceCheckPoint;
		private readonly IEntityRepository<ServiceWorker> RepositoryServiceWorker;
		private readonly IEntityRepository<ControlFormAssign> RepositoryFormAssign;
		private readonly IEntityRepository<ControlFormValue> RepositoryFormValue;

		#region Constructors
		public ControlPresenceMobileCheckHandler(
			IUnitOfWork unitOfWork,
			ISessionData sessionData,
			IBlobRepository blobRepository,
			IEntityRepository<ControlPresence> repository,
			IEntityRepository<ServiceTag> repositoryServiceTag,
			IEntityRepository<ControlItem> repositoryControlItem,
			IEntityRepository<ControlTrack> repositoryControlTrack,
			IEntityRepository<ServiceCheckPoint> repositoryServiceCheckPoint,
			IEntityRepository<ServiceWorker> repositoryServiceWorker,
			IEntityRepository<ControlFormAssign> repositoryFormAssign,
			IEntityRepository<ControlFormValue> repositoryFormValue)
		{
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (blobRepository == null) throw new ArgumentNullException("blobRepository");
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryServiceTag == null) throw new ArgumentNullException("repositoryServiceTag");
			if (repositoryControlItem == null) throw new ArgumentNullException("repositoryControlItem");
			if (repositoryControlTrack == null) throw new ArgumentNullException("repositoryControlTrack");
			if (repositoryServiceCheckPoint == null) throw new ArgumentNullException("repositoryServiceCheckPoint");
			if (repositoryServiceWorker == null) throw new ArgumentNullException("repositoryServiceWorker");
			if (repositoryFormAssign == null) throw new ArgumentNullException("repositoryFormAssign");
			if (repositoryFormValue == null) throw new ArgumentNullException("repositoryFormValue");
			
			UnitOfWork = unitOfWork;
			SessionData = sessionData;
			BlobRepository = blobRepository;
			Repository = repository;
			RepositoryServiceTag = repositoryServiceTag;
			RepositoryControlItem = repositoryControlItem;
			RepositoryControlTrack = repositoryControlTrack;
			RepositoryServiceCheckPoint = repositoryServiceCheckPoint;
			RepositoryServiceWorker = repositoryServiceWorker;
			RepositoryFormAssign = repositoryFormAssign;
			RepositoryFormValue = repositoryFormValue;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlPresenceMobileCheckArguments arguments)
		{
			var now            = DateTime.Now;
			var date           = arguments.Date;
			var yesterday      = date.AddDays(-1);
			var entrances      = new List<ControlTrack>();
			var exits          = new List<ControlTrack>();
			var trackFrecuency = DateTime.MaxValue;

			var tag = await RepositoryServiceTag.GetAsync(arguments.Id);
			if (tag == null)
				throw new ArgumentNullException("id");

			var worker = (await RepositoryServiceWorker.GetAsync())
				.Where(x => x.Login == SessionData.Login)
				.FirstOrDefault();
			if (worker == null)
				throw new ArgumentNullException("sessiondata.login");

			if (arguments.Items != null)
				foreach (var itemArguments in arguments.Items)
				{
					var item = await RepositoryControlItem.GetAsync(itemArguments.Id);
					if (item == null)
						throw new ArgumentNullException("Items.Id");

					var trackFrec = new DateTime(1900, 1, 1, item.TrackFrecuency.Hour, item.TrackFrecuency.Minute, 0, 0);
					var trackFrecMod = new XpDuration(trackFrec);
					var checkPoint = await RepositoryServiceCheckPoint.GetAsync(itemArguments.CheckPointId);
					if (checkPoint == null)
						throw new ArgumentNullException("Items.CheckId");

					var track = (await RepositoryControlTrack.GetAsync("PresenceUntil"))
						.Where(x =>
							x.WorkerId == worker.Id &&
							x.ItemId == itemArguments.Id &&
							x.PresenceSince != null &&
							yesterday < x.PresenceSince.Date && x.PresenceSince.Date <= date
						)
						.OrderByDescending(x => x.PresenceSince.Date)
						.ThenByDescending(x => x.PresenceSince.CreatedAt)
						.FirstOrDefault();

					var type = checkPoint.Type;
					if (type == CheckPointType.Check)
					{
						if ((track == null) || (track.PresenceUntil != null))
							type = CheckPointType.Entrance;
						else
							type = CheckPointType.Exit;
					}
					if ((type == CheckPointType.Entrance) || (type == CheckPointType.Round) || (track == null) || (track.PresenceUntil != null))
					{
						track = new ControlTrack
						{
							CreatedAt = now,
							ItemId = itemArguments.Id,
							WorkerId = worker.Id
						};
						await RepositoryControlTrack.AddAsync(track);
					}
					if (item.SaveTrack && (type != CheckPointType.Round))
					{
						if (type == CheckPointType.Entrance)
						{
							entrances.Add(track);
							if (trackFrec < trackFrecuency)
								trackFrecuency =  trackFrec;
						}
						else
							exits.Add(track);
					}
					if (itemArguments.CheckId != null)
					{
						if (itemArguments.Assigns != null)
							foreach (var AssignArguments in itemArguments.Assigns)
							{
								var Assign = await RepositoryFormAssign.GetAsync(AssignArguments.Id, "Values");
								if (Assign == null)
									throw new ArgumentNullException("Assign.Id");

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
						Latitude = arguments.Latitude,
						LatitudeWanted = checkPoint.Latitude,
						Longitude = arguments.Longitude,
						LongitudeWanted = checkPoint.Longitude,
						Observations = arguments.Observations ?? "",
						CheckPointId = checkPoint.Id,
						PlanningCheckId = itemArguments.CheckId,
						TrackSince = (type == CheckPointType.Entrance || type == CheckPointType.Round) ? track : null,
						TrackUntil = (type == CheckPointType.Exit || type == CheckPointType.Round) ? track : null
					};
					await Repository.AddAsync(presence);

					await UnitOfWork.SaveAsync();

					// Save image
					if (item.SaveFacialRecognition)
						presence.Image = BlobRepository.SaveFile(ControlPresenceResources.FotoShortUrl.FormatString(presence.Id), arguments.Image);
				}

			var result = new ControlPresenceMobileCheckResult
			{
				Entrances = entrances.Select(x => new ControlPresenceMobileCheckResult.Track { Id = x.Id }),
				Exits = exits.Select(x => new ControlPresenceMobileCheckResult.Track { Id = x.Id }),
				TrackFrecuency = trackFrecuency
			};
			return result;
		}
		#endregion ExecuteAsync
	}
}
