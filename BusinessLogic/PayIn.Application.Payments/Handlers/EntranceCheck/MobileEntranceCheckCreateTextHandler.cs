using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("EntranceCheck", "CreateText")]
	[XpAnalytics("EntranceCheck", "CreateText")]
	public class MobileEntranceCheckCreateTextHandler :
		IServiceBaseHandler<MobileEntranceCheckCreateTextArguments>
	{
		[Dependency] public IEntityRepository<EntranceSystem> EntranceSystemRepository { get; set; }
		[Dependency] public IEntityRepository<Entrance> EntranceRepository { get; set; }
		[Dependency] public IEntityRepository<Exhibitor> ExhibitorRepository { get; set; }
		[Dependency] public IEntityRepository<Check> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IPushService PushService { get; set; }

		public class Codes
		{
			public long? EventCode { get; set; }
			public long EntranceCode { get; set; }

			public EntranceSystem EntranceSystem { get; set; }
			public Event Event { get; set; }
			public EntranceType EntranceType { get; set; }
			public Entrance Entrance { get; set; }
		}

		#region GetEntranceInfoFromCodeTextAsync
		public async Task<Codes> GetEntranceInfoFromCodeTextAsync(string code, int? eventId = null)
		{
			// Get entrance system
			var entranceSystems = (await EntranceSystemRepository.GetAsync("Events"))
				.Where(x =>
					x.PaymentConcessions
						.Any(y =>
							y.Concession.Supplier.Login == SessionData.Login ||
							y.PaymentWorkers.Any(z => z.Login == SessionData.Login)
						)
				)
                .ToList();

			// Check codes
			var defaultCode = (Codes)null;
			foreach (var entranceSystem in entranceSystems)
			{
				var codes = entranceSystem.GetEntranceCodeText(code, null);
				if (codes == null)
					continue;

				var entrance = (await EntranceRepository.GetAsync("Event", "EntranceType", "Checks"))
					.Where(x =>
						(
							(x.State == EntranceState.Active) ||
							(x.State == EntranceState.Validated)
						) &&
						(x.Code == codes.EntranceCode) &&
						(x.Event.State == EventState.Active) &&
						((eventId == null) || (x.EventId == eventId)) &&
						(
							(codes.EventCode == null) ||
							(x.Event.Code == codes.EventCode)
						)
					)
					.FirstOrDefault();
				defaultCode = new Codes
				{
					EventCode = codes.EventCode,
					EntranceCode = codes.EntranceCode
				};

				// Entrada no encontrada
				if (entrance == null)
					continue;

				defaultCode.EntranceSystem = entranceSystem;
				defaultCode.Event = entrance.Event;
				defaultCode.EntranceType = entrance.EntranceType;
				defaultCode.Entrance = entrance;
				break;
			}

			return defaultCode;
		}
		#endregion GetEntranceInfoFromCodeTextAsync

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileEntranceCheckCreateTextArguments arguments)
		{
			var now = DateTime.UtcNow;
			var errors = new List<string>();

			// Obtener codes
			var codes = await GetEntranceInfoFromCodeTextAsync(arguments.Code, arguments.EventId);
			if (codes == null)
				throw new ApplicationException(EntranceCheckResources.NoCodeException);
			if (codes.Event == null)
				throw new ApplicationException(EntranceCheckResources.NoEventException);
			if (codes.Entrance == null)
				throw new ApplicationException(EntranceCheckResources.NoEntranceException);

			return await ValidateAsync(
				codes.Entrance,
				codes.EntranceCode,
				arguments.Force,
				arguments.Observations,
				arguments.Type,
				codes.Event
			);
		}
		#endregion ExecuteAsync

		#region ValidateAsync
		public async Task<dynamic> ValidateAsync(Entrance entrance, long entranceCode, bool force, string observations, CheckInType checkInType, Event evento, string name = "", string lastName = "", string vatNumber = "", string email = "", string login = "", string invitationCode = "")
		{
			var now = DateTime.UtcNow;
			var errors = new List<string>();

			// Evento abierto
			if (evento.CheckInStart > now || evento.CheckInEnd < now)
				errors.AddFormat(EntranceCheckResources.EventCheckPeriodOutException);
			else if (entrance.EntranceType != null)
			{
				if (
					(entrance.EntranceType.CheckInStart > now) ||
					(entrance.EntranceType.CheckInEnd < now)
				)
					// Tipo de entrada abierto
					errors.AddFormat(EntranceCheckResources.EntranceTypeCheckPeriodOutException);
				else
				{
					if (
						(entrance.EntranceType.StartDay != null) &&
						(entrance.EntranceType.EndDay != null)
					)
					{
						// Jornada valida
						if (entrance.EntranceType.StartDay < entrance.EntranceType.EndDay)
						{
							// Jornada normal
							if ((entrance.EntranceType.StartDay?.TimeOfDay > now.TimeOfDay) || (entrance.EntranceType.EndDay?.TimeOfDay < now.TimeOfDay))
								errors.AddFormat(EntranceCheckResources.EntranceTypeOutDaylyTimePeriodException, entrance.EntranceType.StartDay?.TimeOfDay, entrance.EntranceType.EndDay?.TimeOfDay);
						}
						else
						{
							// La jornada traspasa las 12 de la noche
							if ((entrance.EntranceType.StartDay?.TimeOfDay < now.TimeOfDay) && (entrance.EntranceType.EndDay?.TimeOfDay > now.TimeOfDay))
								errors.AddFormat(EntranceCheckResources.EntranceTypeOutDaylyTimePeriodException, entrance.EntranceType.StartDay?.TimeOfDay, entrance.EntranceType.EndDay?.TimeOfDay);
						}
					}

					// Número de días
					if (entrance.EntranceType.NumDay != null)
					{
						var firstCheck = entrance.Checks
							.Select(x => (DateTime?)x.TimeStamp)
							.OrderByDescending(x => x)
							.FirstOrDefault();

						if (firstCheck != null)
						{
							var startPeriod = firstCheck.Value;
							if ((entrance.EntranceType.StartDay != null) || (entrance.EntranceType.EndDay != null))
							{
								var startPeriodTime = (entrance.EntranceType.StartDay ?? entrance.EntranceType.EndDay).Value.TimeOfDay;
								startPeriod = firstCheck?.TimeOfDay > startPeriodTime ?
									firstCheck.Value.Date.Add(startPeriodTime) :
									firstCheck.Value.Date.AddDays(-1).Add(startPeriodTime);
							}
							if (startPeriod.AddDays(entrance.EntranceType.NumDay.Value) < now)
								errors.AddFormat(EntranceCheckResources.EntranceTypeNumDaysExceededException, entrance.EntranceType.NumDay, startPeriod);
						}
					}
				}
			}

			// Obtener aforo anterior
			var entranceCount = (await EntranceRepository.GetAsync())
			.Where(x =>
				(x.EventId == evento.Id) &&
				(
					(x.State == EntranceState.Active) ||
					(x.State == EntranceState.Active)
				)
			)
			.Count(x => x.Checks
				.Any(y =>
					(y.Type == CheckInType.In) &&
					(!x.Checks
						.Any(b =>
							(b.Type == CheckInType.Out) &&
							(b.TimeStamp > y.TimeStamp)
						)
					)
				)
			);
			// Entra en caso de Out y que no haya ninguna
			if (entrance.Checks
				.OrderByDescending(x => x.TimeStamp)
				.FirstOrDefault()?.Type != CheckInType.In
			)
				entranceCount++;

			// Control de aforo
			if ((evento.Capacity != null) && (evento.Capacity < entranceCount))
				errors.AddFormat(EntranceCheckResources.EventFullException);

            // Entrada o salida
            var lastCheck = entrance.Checks
				.OrderByDescending(x => x.TimeStamp)
				.FirstOrDefault();
			if (lastCheck != null)
			{
				if ((lastCheck.Type == CheckInType.In) && (checkInType == CheckInType.In))
					errors.AddFormat(EntranceCheckResources.EntranceIsAlreadyInException);
				if ((lastCheck.Type == CheckInType.Out) && (checkInType == CheckInType.Out))
					errors.AddFormat(EntranceCheckResources.EntranceIsOutException);
			}
			else if (checkInType == CheckInType.Out)
				errors.AddFormat(EntranceCheckResources.EntranceIsOutException);

			// Lanzar excepción si existen errores y no ser fuerza
			if ((!force) && (errors.Count() > 0))
				throw new ApplicationException(errors.JoinString("\n"));

			// Crear check
			var item = new Check
			{
				Entrance = entrance,
				Login = SessionData.Login,
				Observations = observations,
				Errors = errors.JoinString("\n"),
				TimeStamp = now,
				Type = checkInType
			};
			await Repository.AddAsync(item);

			// Advise exhibitors
			await ExhibitorAdviseAsync(evento, invitationCode, (entrance.UserName + " " + entrance.LastName).Trim(), entrance.Id);

			return new MobileEntranceCheckCreateResult
			{
                Count = entranceCount,
                Id = item.Id,
                Code = entrance.Code,
                VatNumber = entrance.VatNumber,
                UserName = entrance.UserName,
                LastName = entrance.LastName,
                TimeStamp = now,
                Type = item.Type,
				// Para Razzmatazz
				EventId = entrance.EventId,
				EventName = entrance.Event.Name,
				EventStart = entrance.Event.EventStart,
				EventEnd = entrance.Event.EventEnd,
				EntranceTypeId = entrance.EntranceType.Id,
				EntranceTypeName = entrance.EntranceType.Name,
				EntranceTypeCode = entrance.EntranceType.Code,
				Errors = errors,
				LastCheckTimeStamp = lastCheck?.TimeStamp,
				LastCheckType = lastCheck?.Type
			};
		}
		#endregion ValidateAsync

		#region ExhibitorAdviseAsync
		public async Task ExhibitorAdviseAsync(Event evento, string invitationCode, string invitationName, int entranceId)
		{
			// Avisar invitador
			var exhibitors = (await ExhibitorRepository.GetAsync())
				.Where(x =>
					(x.State == ExhibitorState.Active) &&
					(x.PaymentConcession.Concession.State == ConcessionState.Active) &&
					(x.Events.Any(y => y.Id == evento.Id)) &&
					(x.InvitationCode == invitationCode)
				)
				.Select(x => x.Email);

			await PushService.SendNotification(
				exhibitors,
				NotificationType.ValidateEntrance,
				NotificationState.Actived,
				"Ha entrado al evento tu invitado {0}".FormatString(invitationName),
				"Entrance",
				entranceId.ToString(),
				entranceId
			);
		}
		#endregion ExhibitorAdviseAsync
	}
}
