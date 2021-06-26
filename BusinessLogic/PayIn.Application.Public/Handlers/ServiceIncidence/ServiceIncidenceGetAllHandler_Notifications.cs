using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceIncidenceGetAllHandler_Notifications :
		IQueryBaseHandler<ServiceIncidenceGetArguments_Notifications, ServiceIncidenceGetResult_Notifications>
	{
		[Dependency] public IEntityRepository<ServiceNotification> ServiceNotificationRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceUser> ServiceUserRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ServiceIncidenceGetResult_Notifications>> ExecuteAsync(ServiceIncidenceGetArguments_Notifications arguments)
		{
			var serviceUsers = await ServiceUserRepository.GetAsync();
			
			var items = (await ServiceNotificationRepository.GetAsync())
				.Where(x =>
					x.State != NotificationState.Deleted &&
					x.IncidenceId == arguments.Id
				);

			// Mark readed
			var toMarkRead = items
				.Where(x =>
					x.SenderLogin != SessionData.Login &&
					x.State == NotificationState.Actived
				);
			foreach (var item in toMarkRead)
				item.State = NotificationState.Read;
			await UnitOfWork.SaveAsync();

			// Return values
			var result = items
				.Select(x => new
				{
					x.Id,
					x.State,
					x.ReceiverLogin,
					x.SenderLogin,
					SenderName = x.SenderLogin == "" ? "" : serviceUsers
						.Where(y => y.Login == x.SenderLogin)
						.Select(y => y.Name + " " + y.LastName)
						.FirstOrDefault(),
					//SenderPhotoUrl = x.SenderLogin == "" ? "" : serviceUsers
					//	.Where(y => y.Login == x.SenderLogin)
					//	.Select(y => y.Photo)
					//	.FirstOrDefault(),
					IsMine = (SessionData.Login == x.SenderLogin),
					x.CreatedAt,
					NotificationPhoto = x.PhotoUrl,
					x.Message,
					x.Longitude,
					x.Latitude
				})
				.OrderBy(x => x.CreatedAt)
				.ToList()
				.Select(x => new ServiceIncidenceGetResult_Notifications
				{
					Id = x.Id,
					State = x.State,
					ReceiverLogin = x.ReceiverLogin ?? "",
					SenderLogin = x.SenderLogin ?? "",
					SenderName = x.SenderName ?? "",
					//SenderPhotoUrl = x.SenderPhotoUrl ?? "",
					IsMine = x.IsMine,
					CreatedAt = x.CreatedAt.ToUTC(),
					NotificationPhoto = x.NotificationPhoto,
					Message = x.Message ?? "",
					Longitude = x.Longitude,
					Latitude = x.Latitude
				})
				.Take(1000)
				.ToList();

			return new ResultBase<ServiceIncidenceGetResult_Notifications> { Data = result };
		}
		#endregion ExecuteAsync
	}
}

