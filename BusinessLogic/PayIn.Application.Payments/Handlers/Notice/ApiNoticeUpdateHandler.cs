using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;
using Xp.Infrastructure.Repositories;
using PayIn.Common.Resources;

namespace PayIn.Application.Payments.Handlers
{
    public class ApiNoticeUpdateHandler :
		IServiceBaseHandler<ApiNoticeUpdateArguments>
	{
		[Dependency] public IEntityRepository<Notice> Repository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }
		[Dependency] public IEntityRepository<ServiceNotification> ServiceNotificationRepository { get; set; }
		[Dependency] public IPushService PushService { get; set; }
		[Dependency] public SecurityRepository SecurityRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ApiNoticeUpdateArguments arguments)
		{
			var now = DateTime.Now;
			arguments.Start = arguments.Start ?? XpDateTime.MinValue;
			arguments.End = arguments.End ?? XpDateTime.MaxValue;

			var notice = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();
			if (notice == null)
				throw new ArgumentNullException("id");

            notice
                .SetState(arguments.State)
                .SetName(arguments.Name)
                .SetDescription(arguments.ShortDescription, arguments.Description)
                .SetVisibility(arguments.IsVisible, arguments.Visibility)
                .SetPosition(arguments.Place, arguments.Longitude, arguments.Latitude)
                .SetVisibilityInterval(arguments.Start, arguments.End)
                .SetEvent(arguments.EventId)
                ;

			var oldRouteUrl = notice.RouteUrl;
			var deleteOldFile = false;
			if (arguments.RouteFileGPX != null)
			{
				// Guardar nuevo fichero
				var azureBlob = new AzureBlobRepository();
				var name = notice.Id + "_" + Guid.NewGuid();
				var fileName = NoticeResources.RouteGPXFile.FormatString(name);
				var routeUrl = azureBlob.SaveFile(fileName, arguments.RouteFileGPX);
				notice.RouteUrl = routeUrl;

				deleteOldFile = true;
			}
			else if (oldRouteUrl != "" && (arguments.RouteFileName == ""))
			{
				notice.RouteUrl = "";
				deleteOldFile = true;
			}

			if (deleteOldFile && oldRouteUrl != "")
			{
				// Borrar oldRouteUrl
				var azureBlob = new AzureBlobRepository();

				try
				{
					azureBlob.DeleteFile(oldRouteUrl);
				}
				catch (Exception ex)
				{
					// Lo dejo aquí porque si da un error podríamos hacer posteriores comprobaciones según su tipo de error
				}

			}

			if (
				arguments.SendNotification &&
				arguments.IsVisible &&
				arguments.Visibility == NoticeVisibility.Public &&
				(arguments.Start <= now && arguments.End > now)
			)
			{
				await UnitOfWork.SaveAsync();

				var serviceUsers = (await SecurityRepository.GetUsers()).Data
					.Where(x =>
						x.UserName != SessionData.Login &&
						x.Block == false &&
						x.EmailConfirmed == true
					)
					.ToList();

				var referenceClass = "Notice";
				var referenceId = notice.Id;
				var message = "Noticia actualizada";

				foreach (var serviceUser in serviceUsers)
				{
					// Notification
					var serviceNotification = new ServiceNotification
					{
						Type = NotificationType.EdictCreate,
						State = NotificationState.Actived,
						ReferenceId = referenceId,
						ReferenceClass = referenceClass,
						SenderLogin = SessionData.Login,
						ReceiverLogin = serviceUser.UserName,
						CreatedAt = now,
						IsRead = false,
						Message = message
					};
					await ServiceNotificationRepository.AddAsync(serviceNotification);
					await UnitOfWork.SaveAsync();

					// Push
					var receivers = new List<string>() {
						serviceUser.UserName
					};
					await PushService.SendNotification(receivers, NotificationType.NoticeUpdate, NotificationState.Actived, message, referenceClass, referenceId.ToString(), serviceNotification.Id);
				}
			}

			return notice;
		}
		#endregion ExecuteAsync
	}
}
