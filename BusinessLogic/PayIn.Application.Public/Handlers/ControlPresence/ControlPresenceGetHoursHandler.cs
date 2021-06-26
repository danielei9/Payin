using PayIn.Application.Dto.Arguments.ControlPresence;
using PayIn.Application.Dto.Results.ControlPresence;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPresenceGetHoursHandler :
		IQueryBaseHandler<ControlPresenceGetHoursArguments, ControlPresenceGetHoursResult>
	{
		private readonly IEntityRepository<ControlPlanning> RepositoryControlPlanning;
		private readonly IEntityRepository<ControlTrack> RepositoryControlTrack;
		private readonly IEntityRepository<ServiceWorker> RepositoryServiceWorker;

		#region Constructors
		public ControlPresenceGetHoursHandler(
			IEntityRepository<ControlPlanning> repositoryControlPlanning,
			IEntityRepository<ControlTrack> repositoryControlTrack,
			IEntityRepository<ServiceWorker> repositoryServiceWorker
		)
		{
			if (repositoryControlPlanning == null) throw new ArgumentNullException("repositoryControlPlanning");
			if (repositoryControlTrack == null) throw new ArgumentNullException("repositoryControlTrack");
			if (repositoryServiceWorker == null) throw new ArgumentNullException("repositoryServiceWorker");

			RepositoryControlPlanning = repositoryControlPlanning;
			RepositoryControlTrack = repositoryControlTrack;
			RepositoryServiceWorker = repositoryServiceWorker;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlPresenceGetHoursResult>> ExecuteAsync(ControlPresenceGetHoursArguments arguments)
		{
			var tomorrow = arguments.Until.AddDays(1);

			var worker = await RepositoryServiceWorker.GetAsync(arguments.WorkerId);

			var presences = (await RepositoryControlTrack.GetAsync())
				.Where(x =>
					x.WorkerId == arguments.WorkerId &&
					x.PresenceSince.Date >= arguments.Since &&
					x.PresenceUntil.Date < tomorrow &&
					x.Item.CheckTimetable
				)
				.Select(x => new
				{
					Day = (x.PresenceSince ?? x.PresenceUntil).Date,
					DurationWorked = SqlFunctions.DateDiff("mi", x.PresenceSince.Date, x.PresenceUntil.Date).Value,
					DurationPlanned = 0
				});
			var controlPlannings = (await RepositoryControlPlanning.GetAsync())
				.Where(x =>
					x.WorkerId == arguments.WorkerId &&
					x.Date >= arguments.Since &&
					x.Date < tomorrow &&
					x.Item.CheckTimetable &&
					x.CheckDuration != null
				)
				.Select(x => new
				{
					Day = x.Date,
					DurationWorked = 0,
					DurationPlanned = SqlFunctions.DatePart("hh", x.CheckDuration).Value * 60 + SqlFunctions.DatePart("mi", x.CheckDuration).Value
				});

			var temp = controlPlannings
				.Union(presences)
				.GroupBy(x => SqlFunctions.DatePart("yyyy", x.Day).Value + "-" + SqlFunctions.DatePart("mm", x.Day).Value + "-" + SqlFunctions.DatePart("dd", x.Day).Value)
				.Select(x => new
				{
					Day = x.FirstOrDefault().Day,
					DurationWorked = x.Sum(y => y.DurationWorked) / 60m,
					DurationPlanned = x.Sum(y => y.DurationPlanned) / 60m
				})
				.ToList()
				.Select(x => new ControlPresenceGetHoursResult
				{
					Day = x.Day,
					DurationWorked = x.DurationWorked,
					DurationPlanned = x.DurationPlanned
				});

			var dates = new List<DateTime>();
			for (var date = arguments.Since.Value.Date; date <= arguments.Until.Value.Date; date = date.AddDays(1))
				if (!temp.Where(x => x.Day == date).Any())
					dates.Add(date);

			var result =
				temp
				.Union(
					dates.Select(x => new ControlPresenceGetHoursResult
					{
						Day = x,
						DurationWorked = 0,
						DurationPlanned = 0
					})
				)
				.OrderBy(x => x.Day)
			;

			return new ControlPresenceGetHoursResultBase
			{
				WorkerName = worker.Name,
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}

