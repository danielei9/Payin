using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Domain.Public;
using PayIn.BusinessLogic.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;
using PayIn.Common;
using Microsoft.Practices.Unity;

namespace PayIn.Application.Public.Handlers
{
	// POST Mobile/ServiceNotification/v1
	public class ServiceNotificationCreateHandler :
		IServiceBaseHandler<ServiceNotificationCreateArguments>
	{
		[Dependency] public IEntityRepository<ServiceNotification> Repository { get; set; }
		[Dependency] public IPushService PushService { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		
		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceNotificationCreateArguments arguments)
		{
			var now = DateTime.Now;

			var Notification = new ServiceNotification
			{
				CreatedAt = now,
				Type = arguments.Type,
				State = NotificationState.Actived,
				ReferenceId = arguments.ReferenceId,
				ReferenceClass = arguments.ReferenceClass,
				SenderLogin = arguments.SenderLogin,
				ReceiverLogin = arguments.ReceiverLogin,
				IsRead = false,
				Message = arguments.Message ?? ""
			};
			await Repository.AddAsync(Notification);

			// Solo se envían notificaciones si hay receptores. También sería posible enviarlas, en tal caso, a todos los participantes en la conversación
			if (arguments.ReceiverLogin != "")
			{
				var logins = new List<string>
				{
					arguments.ReceiverLogin
				};

				await PushService.SendNotification(logins, Notification.Type, Notification.State, arguments.Message, arguments.ReferenceClass, arguments.ReferenceId.ToString(), Notification.Id );
			}

			return null;
		}			
		#endregion ExecuteAsync
	}
}
