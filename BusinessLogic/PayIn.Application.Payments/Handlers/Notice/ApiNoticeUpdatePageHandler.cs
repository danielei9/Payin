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

namespace PayIn.Application.Payments.Handlers
{
    public class ApiNoticeUpdatePageHandler :
		IServiceBaseHandler<ApiNoticeUpdatePageArguments>
	{
		[Dependency] public IEntityRepository<Notice> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }
		[Dependency] public IEntityRepository<ServiceNotification> ServiceNotificationRepository { get; set; }
		[Dependency] public IPushService PushService { get; set; }
		[Dependency] public SecurityRepository SecurityRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ApiNoticeUpdatePageArguments arguments)
		{
			var now = DateTime.Now;

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
                .SetSuperNotice(arguments.SuperNoticeId);

			if (
				arguments.SendNotification &&
				arguments.IsVisible &&
				arguments.Visibility == NoticeVisibility.Public
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

				var referenceClass = "Page";
				var referenceId = notice.Id;
				var message = "Página actualizada";

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
					await PushService.SendNotification(receivers, NotificationType.PageUpdate, NotificationState.Actived, message, referenceClass, referenceId.ToString(), serviceNotification.Id);
				}
			}


			return notice;
		}
		#endregion ExecuteAsync
	}
}
