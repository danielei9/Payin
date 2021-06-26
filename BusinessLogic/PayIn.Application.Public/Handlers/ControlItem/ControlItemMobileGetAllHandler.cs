using PayIn.Application.Dto.Arguments.ControlItem;
using PayIn.Application.Dto.Results.ControlItem;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlItemMobileGetAllHandler :
		IQueryBaseHandler<ControlItemMobileGetAllArguments, ControlItemMobileGetAllResult>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<ControlItem> RepositoryControlItem;

		#region Constructors
		public ControlItemMobileGetAllHandler(
			ISessionData sessionData,
			IEntityRepository<ControlItem> repositoryControlItem)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repositoryControlItem == null) throw new ArgumentNullException("repositoryControlItem");

			SessionData = sessionData;
			RepositoryControlItem = repositoryControlItem;
		}
		#endregion Constructor

		#region ExecuteAsync
		async Task<ResultBase<ControlItemMobileGetAllResult>> IQueryBaseHandler<ControlItemMobileGetAllArguments, ControlItemMobileGetAllResult>.ExecuteAsync(ControlItemMobileGetAllArguments arguments)
		{
			var now = DateTime.Now;
			var yesterday = now.AddDays(-1);
			var tomorrow = now.AddDays(1);

			var result = (await RepositoryControlItem.GetAsync())
				.Where(x => x.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login))
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
								y.Worker.Login == SessionData.Login &&
								yesterday < (y.PresenceSince ?? y.PresenceUntil).Date &&
								(y.PresenceSince ?? y.PresenceUntil).Date <= now
							)
						.OrderByDescending(y => (y.PresenceSince ?? y.PresenceUntil).Date)
						.Take(1)
						.Any(y =>
							y.PresenceSince != null &&
							y.PresenceUntil == null
						) ? PresenceType.Exit : PresenceType.Entrance,
					Plannings = x.Plannings
					.Where(y => y.Worker.Login == SessionData.Login)
				})
				.Select(x => new ControlItemMobileGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					Observations = x.Observations,
					SaveTrack = x.SaveTrack,
					SaveFacialRecognition = x.SaveFacialRecognition,
					CheckTimetable = x.CheckTimetable,
					PresenceType = x.PresenceType,
					Plannings = x.Plannings
						.SelectMany(z => (z.Checks
								.Where(y => y.ItemsSince.Count() + y.ItemsUntil.Count() == 0)
								//.Where(a => a.CheckPoint != null && a.CheckPoint.Type == CheckPointType.Round)
								.Select(a => new
								{
									CheckId = (int?)a.Id,
									PresenceType = PresenceType.Round,
									Check = a,
									CheckPointId = a.CheckPointId
								})
							)
							.Union(z.Items
								.Where(a => x.PresenceType == PresenceType.Entrance)
								.Select(a => new
								{
									CheckId = (int?)a.Since.Id,
									PresenceType = PresenceType.Entrance,
									Check = a.Since,
									CheckPointId = a.Since.CheckPointId
								})
							)
							.Union(z.Items
								.Where(a => x.PresenceType == PresenceType.Exit)
								.Select(a => new
								{
									CheckId = (int?)a.Until.Id,
									PresenceType = PresenceType.Exit,
									Check = a.Until,
									CheckPointId = a.Until.CheckPointId
								})
							)
							.Where(a =>
								yesterday < a.Check.Date &&
								a.Check.Date <= tomorrow
							)
							.Select(a => new ControlItemMobileGetAllResult_Planning
							{
								Id = a.Check.Id,
								Date = a.Check.Date,
								CheckId = a.CheckId,
								CheckPointId = a.CheckPointId,
								PresenceType = a.PresenceType,
								EllapsedMinutes = SqlFunctions.DateDiff("mi", a.Check.Date, now).Value, /* No hace falta devolverlo, pero se necesita para el orderby */
								Assigns = a.Check.FormAssigns
									.Select(b => new ControlItemMobileGetAllResult_Assign
									{
										Id = b.Id,
										FormName = b.Form.Name,
										FormObservations = b.Form.Observations,
										Values = b.Values
											.Select(c => new ControlItemMobileGetAllResult_Value
											{
												Id = c.Id,
												Name = c.Argument.Name,
												Type = c.Argument.Type,
												Target = c.Target,
												ValueString = c.ValueString,
												ValueNumeric = c.ValueNumeric,
												ValueBool = c.ValueBool,
												ValueDateTime = c.ValueDateTime
											})
									})
							})
						)
						.OrderBy(a => SqlFunctions.Sign(a.EllapsedMinutes) * a.EllapsedMinutes)
				})
				.ToList()
				.Select(x => new ControlItemMobileGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					Observations = x.Observations,
					SaveTrack = x.SaveTrack,
					SaveFacialRecognition = x.SaveFacialRecognition,
					CheckTimetable = x.CheckTimetable,
					PresenceType = x.PresenceType,
					Plannings = x.Plannings
						.Select(a => new ControlItemMobileGetAllResult_Planning
						{
							Id = a.Id,
							Date = a.Date,
							CheckId = a.CheckId,
							CheckPointId = a.CheckPointId,
							PresenceType = a.PresenceType,
							Assigns = a.Assigns
								.Select(b => new ControlItemMobileGetAllResult_Assign
								{
									Id = b.Id,
									FormName = b.FormName,
									FormObservations = b.FormObservations,
									Values = b.Values
										.Select(c => new ControlItemMobileGetAllResult_Value
										{
											Id = c.Id,
											Name = c.Name,
											Type = c.Type,
											Target = c.Target,
											IsRequired = c.IsRequired,
											ValueString = c.ValueString,
											ValueNumeric = c.ValueNumeric,
											ValueBool = c.ValueBool,
											ValueDateTime = (c.Type == ControlFormArgumentType.Datetime || c.Type == ControlFormArgumentType.Time)
												? ((c.ValueDateTime == null) ? null : c.ValueDateTime.ToUTC())
												: c.ValueDateTime
										})
								})
						})
				});

			return new ResultBase<ControlItemMobileGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
