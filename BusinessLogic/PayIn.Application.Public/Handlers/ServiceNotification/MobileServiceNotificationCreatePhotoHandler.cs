using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileServiceIncidenceCreatePhotoHandler :
		IServiceBaseHandler<MobileServiceIncidenceCreatePhotoArguments>
	{
		[Dependency] public IEntityRepository<ServiceIncidence> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceNotification> ServiceNotificationRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileServiceIncidenceCreatePhotoArguments arguments)
		{
			var now = DateTime.Now;

			var serviceIncidence = (await Repository.GetAsync("Concession.Supplier"))
				.Where(x => x.Id == arguments.IncidenceId)
				.FirstOrDefault();
			if (serviceIncidence == null)
				throw new ApplicationException("No se ha encontrado el emisor del mensaje");

			string senderLogin = SessionData.Login;
			var receiverLogin = serviceIncidence.Concession.Supplier.Login;

			var azureBlob = new AzureBlobRepository();
			byte[] b1 = System.Text.Encoding.UTF8.GetBytes(arguments.PhotoUrl);
			var name = arguments.IncidenceId + "_" + Guid.NewGuid();

			var photoUrl = azureBlob.SaveImage(ServiceNotificationResources.PhotoShortUrl.FormatString(name), b1);

			var serviceNotification = new ServiceNotification
			{
				PhotoUrl = photoUrl,
				Type = NotificationType.ServiceNotificationCreate,
				State = NotificationState.Actived,
				SenderLogin = senderLogin,
				ReceiverLogin = receiverLogin,
				IsRead = false,
				CreatedAt = now,
				Incidence = serviceIncidence,
				Message = senderLogin + " te ha enviado una imagen nueva"
			};			
			await ServiceNotificationRepository.AddAsync(serviceNotification);

			return serviceIncidence;
		}
		#endregion ExecuteAsync
	}
}
