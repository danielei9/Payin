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
	public class ApiNoticeCreateEdictHandler : IServiceBaseHandler<ApiNoticeCreateEdictArguments>
	{
		[Dependency] public IEntityRepository<Notice> Repository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }
		[Dependency] public IEntityRepository<ServiceNotification> ServiceNotificationRepository { get; set; }
		[Dependency] public IPushService PushService { get; set; }
		[Dependency] public SecurityRepository SecurityRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ApiNoticeCreateEdictArguments arguments)
		{
			var now = DateTime.Now;
			arguments.Start = arguments.Start ?? XpDateTime.MinValue;
			arguments.End = arguments.End ?? XpDateTime.MaxValue;

			var paymentConcession = (await PaymentConcessionRepository.GetAsync())
				.Where(x =>
					x.Concession.Supplier.Login == SessionData.Login &&
					x.Concession.State == ConcessionState.Active
				)
				.FirstOrDefault();

            var notice = Notice
				.CreateEdict(paymentConcession.Id, arguments.Name, arguments.ShortDescription, arguments.Description)
				.SetPosition(arguments.Place, arguments.Longitude, arguments.Latitude)
				.SetVisibility(arguments.IsVisible, arguments.Visibility)
				.SetVisibilityInterval(arguments.Start, arguments.End);
			await Repository.AddAsync(notice);

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
				
				var referenceClass = "Edict";
				var referenceId = notice.Id;
				var message = "Nuevo bando";

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
					await PushService.SendNotification(receivers, NotificationType.EdictCreate, NotificationState.Actived, message, referenceClass, referenceId.ToString(), serviceNotification.Id);
				}
			}

			return notice;
		}
		#endregion ExecuteAsync
	}
}
