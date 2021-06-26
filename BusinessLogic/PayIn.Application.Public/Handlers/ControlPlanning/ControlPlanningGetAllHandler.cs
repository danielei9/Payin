using PayIn.Application.Dto.Arguments.ControlPlanning;
using PayIn.Application.Dto.Arguments.ControlPresence;
using PayIn.Application.Dto.Results;
using PayIn.Application.Dto.Results.ControlPresence;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPlanningGetAllHandler :
		IQueryBaseHandler<ControlPlanningGetAllArguments, ControlPlanningGetAllResult>
	{
		private readonly IEntityRepository<ControlPlanning> Repository;
		private readonly IEntityRepository<ControlTrack>    RepositoryControlTrack;
		private readonly IEntityRepository<ControlIncident> RepositoryControlIncident;
		private readonly IEntityRepository<ServiceWorker>   RepositoryServiceWorker;

		#region Constructors
		public ControlPlanningGetAllHandler(
			IEntityRepository<ControlPlanning> repository,
			IEntityRepository<ControlTrack> repositoryControlTrack,
			IEntityRepository<ControlIncident> repositoryControlIncident,
			IEntityRepository<ServiceWorker> repositoryServiceWorker

		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryControlTrack == null) throw new ArgumentNullException("repositoryControlTrack");
			if (repositoryControlIncident == null) throw new ArgumentNullException("repositoryControlIncident");
			if (repositoryServiceWorker == null) throw new ArgumentNullException("repositoryServiceWorker");

			Repository = repository;
			RepositoryControlTrack = repositoryControlTrack;
			RepositoryControlIncident = repositoryControlIncident;
			RepositoryServiceWorker = repositoryServiceWorker;

		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlPlanningGetAllResult>> ExecuteAsync(ControlPlanningGetAllArguments arguments)
		{
			var incidents = await RepositoryControlIncident.GetAsync();
				
			var worker = await RepositoryServiceWorker.GetAsync(arguments.WorkerId);

			var traces = (await RepositoryControlTrack.GetAsync())
				.Where(x => 
					x.WorkerId == arguments.WorkerId &&
					(x.PresenceSince ?? x.PresenceUntil).Date >= arguments.Since &&
					(x.PresenceSince ?? x.PresenceUntil).Date <= arguments.Until
				)
				.Select(x => new
				{
					Id = x.Id,
					Title = x.Item.Name,
					Info = x.Item.Observations,
					Location = (x.PresenceSince ?? x.PresenceUntil).Latitude + ", " + (x.PresenceSince ?? x.PresenceUntil).Longitude,
					Start = x.PresenceSince == null ? (DateTime?)null : x.PresenceSince.Date,
					End = x.PresenceUntil == null ? (DateTime?)null : x.PresenceUntil.Date,
					ItemId = x.ItemId,
					WorkerId = x.WorkerId,
					WorkerName = x.Worker.Name,
					CheckTimetable = x.Item.CheckTimetable,
					EntranceCheckType = x.PresenceSince == null ? CheckType.Undefined : x.PresenceSince.CheckPoint.Tag != null ? CheckType.NFC : CheckType.Manual,
					ExitCheckType = x.PresenceUntil == null ? CheckType.Undefined : x.PresenceUntil.CheckPoint.Tag != null ? CheckType.NFC  : CheckType.Manual
				})
				.ToList()
				.Select(x => new ControlPlanningGetAllResult_Item
				{
					Id = x.Id,
					Title = x.Title,
					Type = CalendarItemResult.Types.Presence,
					Info = x.Info,
					Location = x.Location,
					Start = x.Start, // Need to be done in memory
					End = x.End, // Need to be done in memory
					ItemId = x.ItemId,
					WorkerId = x.WorkerId,
					WorkerName = x.WorkerName,
					CheckTimetable = x.CheckTimetable,
					EntranceCheckType = x.EntranceCheckType,
					ExitCheckType = x.ExitCheckType
				});

			var plannings = (await Repository.GetAsync())
				.Where(x =>
					x.WorkerId == arguments.WorkerId &&
					x.Date >= arguments.Since &&
					x.Date <= arguments.Until)
				.Select(x => new
				{
					Id = x.Id,
					Date = x.Date,
					CheckDuration = x.CheckDuration,				
					Items =
						(
							x.Items
							.Select(y => new
							{
								Id = y.Id,
								Type = CalendarItemResult.Types.Planning,
								Title = x.Item.Name,
								Info = x.Item.Observations,
								Start = y.Since.Date,
								End = (DateTime?)y.Until.Date,
								ItemId = x.ItemId,
								ItemName = x.Item.Name,
								WorkerId = x.WorkerId,
								WorkerName = x.Worker.Name
							})
						)
						.Union(
							x.Checks
							.Where(y => y.ItemsSince.Count == 0 && y.ItemsUntil.Count == 0)
							.Select(y => new
							{
								Id = y.Id,
								Type = CalendarItemResult.Types.Check,
								Title = x.Item.Name,
								Info = x.Item.Observations,
								Start = y.Date,
								End = (DateTime?)null,
								ItemId = x.ItemId,
								ItemName = x.Item.Name,
								WorkerId = x.WorkerId,
								WorkerName = x.Worker.Name
							})
						)
				})
				.ToList()
				.Select(x => new ControlPlanningGetAllResult
				{
					Id = x.Id,
					Date = x.Date, // Need to be done in memory					
					CheckDuration = x.CheckDuration, // Need to be done in memory	
					Items = x.Items
						.OrderBy(y => y.Start)
						.Select(y => new ControlPlanningGetAllResult_Item
						{
							Id = y.Id,
							Type = y.Type,
							Title = y.Title,
							Info = y.Info,
							Location = "",
							Start = y.Start, // Need to be done in memory
							End = y.End, // Need to be done in memory
							ItemId = y.ItemId,
							WorkerId = y.WorkerId,
							WorkerName = y.WorkerName
						})
				});

			var dates = new List<DateTime>();
			for (var date = arguments.Since.Value.Date; date <= arguments.Until.Value.Date; date = date.AddDays(1))
				if (!plannings.Where(x => x.Date == date).Any())
					dates.Add(date);

			var result = plannings
				.Union(dates
					.Select(x => new ControlPlanningGetAllResult {
						Id = 0,
						SumChecks = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
						CheckDuration = null,
						Date = x
					})
				)
				.OrderBy(x => x.Date.Value)
				.Select(x => new ControlPlanningGetAllResult
				{
					Id = x.Id,
					SumChecks = (TimeSpan.FromMinutes(
						traces
							.Where(y => 
								(y.Start ?? y.End).Value.Date == x.Date &&
								y.CheckTimetable
							)
							.Sum(y => y.Duration.TotalMinutes)
					)),
					CheckDuration = x.CheckDuration,
					Date = x.Date,
					Items = x.Items
						.Union(traces
							.Where(y => 
								(y.Start ?? y.End).Value.Date == x.Date
							)
						)
						.OrderBy(y => (y.Start ?? y.End).Value)
				})
				.ToList();

			return new ControlPlanningGetAllResultBase { Data = result, WorkerName = worker.Name };
		}
		#endregion ExecuteAsync
	}
}
