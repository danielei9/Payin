using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.Notification;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Public.Handlers
{
	// POST Mobile/ServiceNotification/v2
	public class MobileServiceNotificationCreateHandler :
		IServiceBaseHandler<MobileServiceNotificationCreateArguments>
	{
		[Dependency] public IEntityRepository<ServiceNotification> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceIncidence> IncidenceRepository { get; set; }
		[Dependency] public IPushService PushService { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public SecurityRepository Security { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileServiceNotificationCreateArguments arguments)
		{
			int incidenceId = arguments.IncidenceId;
			if (incidenceId <= 0)
				throw new ArgumentException("IncidenceId");

			if (arguments.Message == null && arguments.Latitude == null && arguments.Longitude == null && arguments.PhotoUrl == null)
				throw new ApplicationException("El mensaje no puede estar vacío");

			if ((arguments.Message == null && (arguments.Latitude == null || arguments.Longitude == null)) && arguments.PhotoUrl == "")
				throw new ApplicationException("La ubicación está incompleta");

			var now = DateTime.Now;

			string sender = SessionData.Login;
			var receivers = (await IncidenceRepository.GetAsync())
				.Where(x =>
					x.Id == incidenceId &&
					x.State == IncidenceState.Active
				)
				.SelectMany(x =>
					(
						x.Notifications
							.Where(y =>
								(y.State == NotificationState.Actived || y.State == NotificationState.Read) &&
								(y.ReceiverLogin != SessionData.Login)
							)
							.Select(y => y.ReceiverLogin)
					).Union(
						x.Notifications
							.Where(y => 
								(y.State == NotificationState.Actived || y.State == NotificationState.Read) &&
								(y.SenderLogin != SessionData.Login)
							)
							.Select(y => y.SenderLogin)
					)
				)
				.ToList();


			var notification = new ServiceNotification
			{
				CreatedAt = now,
				IncidenceId = incidenceId,
				ReferenceId = incidenceId,
				ReferenceClass = "ServiceIncidence",
				Type = NotificationType.ServiceNotificationCreate,
				State = NotificationState.Actived,
				IsRead = false,
				SenderLogin = sender,
				ReceiverLogin = receivers.JoinString(","),
				Longitude = arguments.Longitude,
				Latitude = arguments.Latitude,
				PhotoUrl = "", // Se asigna bajo
				Message = arguments.Message ?? sender + " ha enviado una nueva ubicación para una incidencia"
			};

			// Guardar la foto, si se ha subido alguna
			if (!arguments.PhotoUrl.IsNullOrEmpty())
			{
				var azureBlob = new AzureBlobRepository();
				byte[] b1 = System.Text.Encoding.UTF8.GetBytes(arguments.PhotoUrl);
				var name = incidenceId + "_" + Guid.NewGuid();

				notification.PhotoUrl = azureBlob.SaveImage(ServiceNotificationResources.PhotoShortUrl.FormatString(name), b1);
			}
			await Repository.AddAsync(notification);
			await UnitOfWork.SaveAsync();

			// Solo se envían notificaciones si hay receptores. También sería posible enviarlas, en tal caso, a todos los participantes en la conversación
			if (receivers.Count() > 0)
			{
				string relatedName = "ServiceIncidence";
				string relatedId = incidenceId.ToString();

				await PushService.SendNotification(receivers, notification.Type, notification.State, arguments.Message, relatedName, relatedId, notification.Id);
			}

			return notification;
		}			
		#endregion ExecuteAsync
	}
}
