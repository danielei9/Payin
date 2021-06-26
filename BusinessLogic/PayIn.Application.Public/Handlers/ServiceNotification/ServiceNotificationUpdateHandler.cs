using System;
using System.Collections.Generic;
using System.Linq;
using PayIn.BusinessLogic.Common;
using System.Text;
using System.Threading.Tasks;
using PayIn.Application.Dto.Arguments.ServiceNotification;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Public;

namespace PayIn.Application.Public.Handlers.Notifications
{
	public class ServiceNotificationUpdateHandler :
		IServiceBaseHandler<ServiceNotificationUpdateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<ServiceWorker> Repository;
		private readonly IEntityRepository<ServiceNotification> RepositoryNotifications;

		#region Constructors
		public ServiceNotificationUpdateHandler(
			ISessionData sessionData,
			IEntityRepository<ServiceWorker> repository,
			IEntityRepository<ServiceNotification> repositoryNotifications
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryNotifications == null) throw new ArgumentNullException("repositoryNotifications");

			SessionData = sessionData;
			Repository = repository;
			RepositoryNotifications = repositoryNotifications;
		}
		#endregion Constructors
		
		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceNotificationUpdateArguments>.ExecuteAsync(ServiceNotificationUpdateArguments arguments)
		{
			var worker = (await Repository.GetAsync())
					.Where(x =>
						 (x.Login == SessionData.Login)
					)
					.FirstOrDefault();
			if (worker == null)
				throw new ArgumentNullException("sessiondata.login");
			var Notification = (await RepositoryNotifications.GetAsync())
				.Where(x => x.Id == arguments.NotificationId)
				.FirstOrDefault();
			if (arguments.Type == NotificationType.AcceptAssignment) // && arguments.Item.HasAccepted == true
			{
				worker.HasAccepted = true;
				Notification.State = NotificationState.Read;
			}
		return true;
		}
		#endregion ExecuteAsync
	}
}


