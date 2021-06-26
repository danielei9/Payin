using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileServiceIncidenceCreatePositionHandler :
		IServiceBaseHandler<MobileServiceIncidenceCreatePositionArguments>
	{
		[Dependency] public IEntityRepository<ServiceIncidence> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceNotification> ServiceNotificationRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileServiceIncidenceCreatePositionArguments arguments)
		{
			var now = DateTime.Now;

			var serviceIncidence = (await Repository.GetAsync("Concession.Supplier"))
				.Where(x => x.Id == arguments.IncidenceId)
				.FirstOrDefault();
			if (serviceIncidence == null)
				throw new ApplicationException("No se ha encontrado el emisor del mensaje");

			string senderLogin = SessionData.Login;
			var receiverLogin = serviceIncidence.Concession.Supplier.Login;
			
			var serviceNotification = new ServiceNotification
			{
				PhotoUrl = "",
				Type = NotificationType.ServiceNotificationCreate,
				State = NotificationState.Actived,
				SenderLogin = senderLogin,
				ReceiverLogin = receiverLogin,
				IsRead = false,
				CreatedAt = now,
				Incidence = serviceIncidence,
				Longitude = arguments.Longitude,
				Latitude = arguments.Latitude
			};			
			await ServiceNotificationRepository.AddAsync(serviceNotification);

			return serviceIncidence;
		}
		#endregion ExecuteAsync
	}
}
