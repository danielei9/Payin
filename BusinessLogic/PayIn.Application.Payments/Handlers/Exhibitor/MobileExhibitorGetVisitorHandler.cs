using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class MobileExhibitorGetVisitorHandler :
		IQueryBaseHandler<MobileExhibitorGetVisitorArguments, MobileExhibitorGetVisitorResult>
	{
		[Dependency] public IEntityRepository<Contact> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceNotification> ServiceNotificationRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobileExhibitorGetVisitorResult>> ExecuteAsync(MobileExhibitorGetVisitorArguments arguments)
		{
			var notifications = await ServiceNotificationRepository.GetAsync();

			var result = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					x.Id,
					UserName = x.VisitorName,
					Login = x.VisitorLogin,
					EventName = x.Event.Name,
					EntranceId = x.VisitorEntranceId,
					Notifications = notifications
						.Where(y =>
							(
								(
									(y.SenderLogin == x.VisitorLogin) &&
									(y.ReceiverLogin == SessionData.Login)
								) || (
									(y.ReceiverLogin == x.VisitorLogin) &&
									(y.SenderLogin == SessionData.Login)
								)
							) &&
							y.State != NotificationState.Deleted &&
							y.Type == NotificationType.ChatSend
						)
						.OrderByDescending(y => y.CreatedAt)
						.Select(y => new
						{
							y.Id,
							y.CreatedAt,
							y.Message,
							y.SenderLogin,
							y.ReceiverLogin,
							y.State
						})
				})
				.ToList();

			var result2 = result
				.Select(x => new MobileExhibitorGetVisitorResult
				{
					Id = x.Id,
					UserName = x.UserName,
					Login = x.Login,
					EventName = x.EventName,
					EntranceId = x.EntranceId,
					Notifications = x.Notifications
						.ToList()
						.Select(y => new MobileExhibitorGetVisitorResult_Notification
						{
							Id = y.Id,
							CreatedAt = y.CreatedAt,
							Message = y.Message,
							SenderLogin = y.SenderLogin,
							ReceiverLogin = y.ReceiverLogin,
							State = y.State
						})
				});

			return new ResultBase<MobileExhibitorGetVisitorResult> { Data = result2 };
		}
		#endregion ExecuteAsync
	}
}
