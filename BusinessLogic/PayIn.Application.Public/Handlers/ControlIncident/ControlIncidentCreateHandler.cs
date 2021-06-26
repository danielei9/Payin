using PayIn.Application.Dto.Arguments.ControlIncident;
using PayIn.Application.Dto.Results.ControlIncident;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using serV = PayIn.Domain.Public.ServiceWorker;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Common.Exceptions;
using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Application.Public.Handlers
{
	public class ControlIncidentCreateHandler :
		IServiceBaseHandler<ControlIncidentCreateArguments>
	{
		private readonly IUnitOfWork UnitOfWork;
		private readonly ISessionData SessionData;
		private readonly IBlobRepository BlobRepository;
		private readonly IEntityRepository<ControlIncident> Repository;
		private readonly IEntityRepository<ControlItem> RepositoryControlItem;
		private readonly IEntityRepository<ControlPresence> RepositoryControlPresence;
		private readonly IEntityRepository<ControlTrack> RepositoryControlTrack;
		private readonly IEntityRepository<ServiceWorker> RepositoryServiceWorker;
		private readonly IEntityRepository<ControlPlanningCheck> RepositoryPlanningCheck;
		private readonly IEntityRepository<ControlPlanningItem> RepositoryPlanningItem;
		private readonly IEntityRepository<ControlFormAssign> RepositoryFormAssign;
		private readonly IEntityRepository<ControlFormValue> RepositoryFormValue;

		#region Constructors
		public ControlIncidentCreateHandler(
			IUnitOfWork unitOfWork,
			ISessionData sessionData,
			IBlobRepository blobRepository,
			IEntityRepository<ControlIncident> repository,
			IEntityRepository<ControlItem> repositoryControlItem,
			IEntityRepository<ControlPresence> repositoryControlPresence,
			IEntityRepository<ControlTrack> repositoryControlTrack,
			IEntityRepository<ServiceWorker> repositoryServiceWorker,
			IEntityRepository<ControlPlanningCheck> repositoryPlanningCheck,
			IEntityRepository<ControlPlanningItem>  repositoryPlanningItem,
			IEntityRepository<ControlFormAssign>    repositoryFormAssign,
			IEntityRepository<ControlFormValue>     repositoryFormValue			
		)
		{
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (blobRepository == null) throw new ArgumentNullException("blobRepository");
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryControlItem == null) throw new ArgumentNullException("repositoryControlItem");
			if (repositoryControlPresence == null) throw new ArgumentNullException("repositoryControlPresence");
			if (repositoryControlTrack == null) throw new ArgumentNullException("repositoryControlTrack");
			if (repositoryServiceWorker == null) throw new ArgumentNullException("repositoryServceWorker");
			if (repositoryPlanningCheck == null) throw new ArgumentNullException("repositoryPlanningCheck");
			if (repositoryPlanningItem == null) throw new ArgumentNullException("repositoryPlanningItem");
			if (repositoryFormAssign == null) throw new ArgumentNullException("repositoryFormAssign");
			if (repositoryFormValue == null) throw new ArgumentNullException("repositoryFormValue");

			UnitOfWork = unitOfWork;
			SessionData = sessionData;
			BlobRepository = blobRepository;
			Repository = repository;
			RepositoryControlItem = repositoryControlItem;
			RepositoryControlPresence = repositoryControlPresence;
			RepositoryControlTrack = repositoryControlTrack;
			RepositoryServiceWorker = repositoryServiceWorker;
			RepositoryPlanningCheck = repositoryPlanningCheck;
			RepositoryPlanningItem = repositoryPlanningItem;
			RepositoryFormAssign = repositoryFormAssign;
			RepositoryFormValue = repositoryFormValue;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlIncidentCreateArguments arguments)
		{
			var now = DateTime.Now;
			var date = arguments.Item.Date;
			var yesterday = date.AddDays(-1);
			var entrances = new List<ControlTrack>();
			var exits = new List<ControlTrack>();
			
			var worker = (await RepositoryServiceWorker.GetAsync())
				.Where(x => x.Login == SessionData.Login)
				.FirstOrDefault(); 
			if (worker == null)
				throw new ArgumentNullException("sessiondata.login");

			var item = await RepositoryControlItem.GetAsync(arguments.Item.ItemId);
			if (item == null)
				throw new ArgumentNullException("Items.Id");

			var track = (await RepositoryControlTrack.GetAsync("PresenceUntil"))
				.Where(x =>
					x.WorkerId == worker.Id &&
					x.ItemId == arguments.Item.ItemId &&
					x.PresenceSince != null &&
					yesterday < x.PresenceSince.Date && x.PresenceSince.Date <= date
				)
				.OrderByDescending(x => x.PresenceSince.Date)
				.ThenByDescending(x => x.PresenceSince.CreatedAt)
				.FirstOrDefault();

			CheckPointType type;
			if ((track == null) || (track.PresenceUntil != null))
				type = CheckPointType.Entrance;
			else
				type = CheckPointType.Exit;
			if ((type == CheckPointType.Entrance) || (track == null) || (track.PresenceUntil != null))
			{
				track = new ControlTrack
				{
					CreatedAt = now,
					ItemId = arguments.Item.ItemId,
					WorkerId = worker.Id
				};
				await RepositoryControlTrack.AddAsync(track);
			}
			if (item.SaveTrack)
			{
				if (type == CheckPointType.Entrance)
					entrances.Add(track);
				else
					exits.Add(track);
			}

			if (arguments.PlanningItems != null)
				foreach (var planningItemArguments in arguments.PlanningItems)
				{
					var PlanningItem = await RepositoryPlanningItem.GetAsync(planningItemArguments.Id);
					if (PlanningItem == null)
						throw new ArgumentNullException("PlanningItem.Id");

					foreach (var planningItemCheckArguments in planningItemArguments.Checks)
					{
						var PlanningItemCheck = await RepositoryPlanningCheck.GetAsync(planningItemCheckArguments.Id);
						if (PlanningItemCheck == null)
							throw new ArgumentNullException("PlanningCheck.Id");

						foreach (var AssignArguments in planningItemCheckArguments.Assigns)
						{
							var Assign = await RepositoryFormAssign.GetAsync(AssignArguments.Id);
							if (Assign == null)
								throw new ArgumentNullException("Assign.Id");

							foreach (var ValueArguments in Assign.Values)
							{
								var Value = await RepositoryFormValue.GetAsync(ValueArguments.Id);
								if (Value == null)
									throw new ArgumentNullException("Value.Id");
								if (Value.Target == ControlFormArgumentTarget.Check)
								{
									if (Value.Argument.Type == ControlFormArgumentType.Bool)
										Value.ValueBool = ValueArguments.ValueBool;
									if (Value.Argument.Type == ControlFormArgumentType.Int || Value.Argument.Type == ControlFormArgumentType.Double)
										Value.ValueNumeric = ValueArguments.ValueNumeric;
									if (Value.Argument.Type == ControlFormArgumentType.String)
										Value.ValueString = ValueArguments.ValueString;
									if (Value.Argument.Type == ControlFormArgumentType.Datetime || Value.Argument.Type == ControlFormArgumentType.Duration || Value.Argument.Type == ControlFormArgumentType.Time || Value.Argument.Type == ControlFormArgumentType.Date)
										Value.ValueDateTime = ValueArguments.ValueDateTime;
								}
							}

						}
					}
				}

			if (arguments.PlanningChecks != null)
				foreach (var planningCheckArguments in arguments.PlanningChecks)
				{
					var PlanningCheck = await RepositoryPlanningCheck.GetAsync(planningCheckArguments.Id);
					if (PlanningCheck == null)
						throw new ArgumentNullException("PlanningCheck.Id");

					foreach (var AssignArguments in planningCheckArguments.Assigns)
					{
						var Assign = await RepositoryFormAssign.GetAsync(AssignArguments.Id);
						if (Assign == null)
							throw new ArgumentNullException("Assign.Id");

						foreach (var ValueArguments in Assign.Values)
						{
							var Value = await RepositoryFormValue.GetAsync(ValueArguments.Id);
							if (Value == null)
								throw new ArgumentNullException("Value.Id");
							if (Value.Target == ControlFormArgumentTarget.Check)
							{
								if (Value.Argument.Type == ControlFormArgumentType.Bool)
									Value.ValueBool = ValueArguments.ValueBool;
								if (Value.Argument.Type == ControlFormArgumentType.Int || Value.Argument.Type == ControlFormArgumentType.Double)
									Value.ValueNumeric = ValueArguments.ValueNumeric;
								if (Value.Argument.Type == ControlFormArgumentType.String)
									Value.ValueString = ValueArguments.ValueString;
								if (Value.Argument.Type == ControlFormArgumentType.Datetime || Value.Argument.Type == ControlFormArgumentType.Duration || Value.Argument.Type == ControlFormArgumentType.Time || Value.Argument.Type == ControlFormArgumentType.Date)
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
				Longitude = arguments.Item.Longitude,
				Observations = arguments.Observations ?? "",
				TrackSince = type == CheckPointType.Entrance ? track : null,
				TrackUntil = type == CheckPointType.Exit ? track : null
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

			var result = new ControlIncidentCreateResult
			{
				Entrances = entrances.Select(x => new ControlIncidentCreateResult.Track { Id = x.Id }),
				Exits = exits.Select(x => new ControlIncidentCreateResult.Track { Id = x.Id })
			};
			return result;
		}
		#endregion ExecuteAsync
	}
}
