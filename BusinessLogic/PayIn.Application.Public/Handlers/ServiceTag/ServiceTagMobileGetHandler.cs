using PayIn.Application.Dto.Arguments.ServiceTag;
using PayIn.Application.Dto.Results.ServiceTag;
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
	public class ServiceTagMobileGetHandler :
		IQueryBaseHandler<ServiceTagMobileGetArguments, ServiceTagMobileGetResult>
	{
		private readonly IEntityRepository<ServiceTag> Repository;
		private readonly IEntityRepository<ControlPresence> RepositoryControlPresence;

		#region Constructors
		public ServiceTagMobileGetHandler(
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
		public async Task<ResultBase<ServiceTagMobileGetResult>> ExecuteAsync(ServiceTagMobileGetArguments arguments)
		{
			var now = DateTime.Now;
			var yesterday = now.AddDays(-1);
			var tomorrow = now.AddDays(1);

			var items = await Repository.GetAsync();

			var result = items
				.Where(x => x.Reference == arguments.Reference)
				.Select(x => new ServiceTagMobileGetResult
				{
					Id = x.Id,
					Items = x.CheckPoints
						.Select(y => new
						{
							Id = y.ItemId,
							Name = y.Item.Name,
							Observations = y.Item.Observations,
							SaveFacialRecognition = y.Item.SaveFacialRecognition,
							SaveTrack = y.Item.SaveTrack,
							CheckPointId = y.Id,
							CheckPointType = y.Type,
							PresenceType =
								y.Type == CheckPointType.Entrance ? PresenceType.Entrance :
								y.Type == CheckPointType.Exit ? PresenceType.Exit :
								y.Type == CheckPointType.Round ? PresenceType.Round :
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
									) ? PresenceType.Exit : PresenceType.Entrance,
							Plannings = y.Item.Plannings
						})
						.Select(y => new ServiceTagMobileGetResult_Item
						{
							Id = y.Id,
							Name = y.Name,
							SaveFacialRecognition = y.SaveFacialRecognition,
							SaveTrack = y.SaveTrack,
							CheckPointId = y.CheckPointId,
							PresenceType = y.PresenceType,
							Plannings = y.Plannings
								.SelectMany(z => (z.Checks
										.Where(a => y.CheckPointType == CheckPointType.Round && a.CheckPointId == y.CheckPointId)
										.Select(a => new
										{
											CheckId = (int?)a.Id,
											PresenceType = PresenceType.Round,
											Check = a
										})
									)
									.Union(z.Items
										.Where(a => y.CheckPointType != CheckPointType.Entrance || y.CheckPointType != CheckPointType.Check)
										.Select(a => new
										{
											CheckId = (int?)a.Since.Id,
											PresenceType = PresenceType.Entrance,
											Check = a.Since
										})
									)
									.Union(z.Items
										.Where(a => y.CheckPointType != CheckPointType.Exit || y.CheckPointType != CheckPointType.Check)
										.Select(a => new
										{
											CheckId = (int?)a.Until.Id,
											PresenceType = PresenceType.Exit,
											Check = a.Until
										})
									)
									.Where(a =>
										yesterday < a.Check.Date &&
										a.Check.Date <= tomorrow
									)
									.Select(a => new ServiceTagMobileGetResult_Planning
									{
										Id = a.Check.Id,
										Date = a.Check.Date,
										CheckId = a.CheckId,
										PresenceType = a.PresenceType,
										EllapsedMinutes = SqlFunctions.DateDiff("mi", a.Check.Date, now).Value, /* No hace falta devolverlo, pero se necesita para el orderby */
										Assigns = a.Check.FormAssigns
											.Select(b => new ServiceTagMobileGetResult_Assign
											{
												Id = b.Id,
												FormName = b.Form.Name,
												FormObservations = b.Form.Observations,
												Values = b.Values
													.Select(c => new ServiceTagMobileGetResult_Value
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
				})
				.ToList()
				.Select(x => new ServiceTagMobileGetResult
				{
					Id = x.Id,
					Items = x.Items
						.Select(y => new ServiceTagMobileGetResult_Item
						{
							Id = y.Id,
							Name = y.Name,
							SaveFacialRecognition = y.SaveFacialRecognition,
							SaveTrack = y.SaveTrack,
							CheckPointId = y.CheckPointId,
							PresenceType = y.PresenceType,
							Plannings = y.Plannings
								.Select(a => new ServiceTagMobileGetResult_Planning
								{
									Id = a.Id,
									Date = a.Date,
									CheckId = a.CheckId,
									PresenceType = a.PresenceType,
									Assigns = a.Assigns
										.Select(b => new ServiceTagMobileGetResult_Assign
										{
											Id = b.Id,
											FormName = b.FormName,
											FormObservations = b.FormObservations,
											Values = b.Values
												.Select(c => new ServiceTagMobileGetResult_Value
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
						})
				});

			return new ResultBase<ServiceTagMobileGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
