using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Domain.Public;
using PayIn.BusinessLogic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;
using PayIn.Common;
using Microsoft.Practices.Unity;
using PayIn.Domain.Payments;

namespace PayIn.Application.Public.Handlers
{
	public class MobileServiceNotificationCreateChatToExhibitorHandler :
		IServiceBaseHandler<MobileServiceNotificationCreateChatToExhibitorArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<ServiceNotification> Repository { get; set; }
		[Dependency] public IEntityRepository<Exhibitor> ExhibitorRepository { get; set; }
		[Dependency] public IPushService PushService { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileServiceNotificationCreateChatToExhibitorArguments arguments)
		{
			var now = DateTime.Now;

			var exhibitorId = (await ExhibitorRepository.GetAsync())
				.Where(x =>
					x.State == ExhibitorState.Active &&
					x.PaymentConcession.Concession.State == ConcessionState.Active &&
					x.PaymentConcession.Concession.Supplier.Login == arguments.ReceiverLogin
				)
				.Select(x => (int?)x.Id)
				.FirstOrDefault();
			if (exhibitorId == null)
				throw new ArgumentNullException("ReceiverLogin");

			var notification = new ServiceNotification
			{
				CreatedAt = now,
				Type = NotificationType.ChatSend,
				State = NotificationState.Actived,
				ReferenceId = exhibitorId,
				ReferenceClass = nameof(Exhibitor),
				SenderLogin = SessionData.Login,
				ReceiverLogin = arguments.ReceiverLogin,
				IsRead = false,
				Message = arguments.Message ?? ""
			};
			await Repository.AddAsync(notification);

			var logins = new List<string>
			{
				arguments.ReceiverLogin
			};

			await PushService.SendNotification(logins, notification.Type, notification.State, notification.Message, notification.ReferenceClass, notification.ReferenceId.ToString(), notification.Id );

			return null;
		}			
		#endregion ExecuteAsync
	}
}
