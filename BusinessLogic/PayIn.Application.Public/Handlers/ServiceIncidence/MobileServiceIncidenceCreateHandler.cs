using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Public.Handlers
{
	// POST MobileServiceIncidence/v1
	public class MobileServiceIncidenceCreateHandler :
		IServiceBaseHandler<MobileServiceIncidenceCreateArguments>
	{
		[Dependency] public IEntityRepository<ServiceIncidence> ServiceIncidenceRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceNotification> ServiceNotificationRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IPushService PushService { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileServiceIncidenceCreateArguments arguments)
		{
			var now = XpDateTime.FromDateTime(DateTime.Now);

			if (arguments.Name == null && arguments.Latitude == null && arguments.Longitude == null && arguments.PhotoUrl == null)
				throw new ApplicationException("El título no puede estar vacío");

			if ((arguments.Name == null && (arguments.Latitude == null || arguments.Longitude == null)) && arguments.PhotoUrl == "")
				throw new ApplicationException("La ubicación está incompleta");


			var receiver = (await PaymentConcessionRepository.GetAsync())
				.Where(x => x.Id == arguments.PaymentConcessionId)
				.Select(x => new
				{
					x.ConcessionId,
					x.Concession.Supplier.Login
				})
				.FirstOrDefault();
			if (receiver == null || receiver.Login == "")
				throw new ApplicationException("No se ha encontrado el receptor del mensaje");

			var serviceIncidence = new ServiceIncidence
			{
				ConcessionId = receiver.ConcessionId,
				Type = arguments.Type,
				Category = arguments.Category,
				SubCategory = arguments.SubCategory,
				DateTime = XpDateTime.FromDateTime(now),
				State = IncidenceState.Active,
				Name = arguments.Name
			};
			await ServiceIncidenceRepository.AddAsync(serviceIncidence);
			await UnitOfWork.SaveAsync();

			// Guardar la foto, si se ha subido alguna
			var photoUrl = "";
			if (arguments.PhotoUrl != "")
			{
				var azureBlob = new AzureBlobRepository();
				byte[] b1 = System.Text.Encoding.UTF8.GetBytes(arguments.PhotoUrl);
				var name = serviceIncidence.Id + "_" + Guid.NewGuid();

				photoUrl = azureBlob.SaveImage(ServiceNotificationResources.PhotoShortUrl.FormatString(name), b1);
			}

			var serviceNotification = new ServiceNotification
			{
				Type = NotificationType.ServiceIncidenceCreate,
				State = NotificationState.Actived,
				ReferenceId = serviceIncidence.Id,
				ReferenceClass = "ServiceIncidence",
				SenderLogin = SessionData.Login,
				ReceiverLogin = receiver.Login,
				CreatedAt = now,
				IsRead = false,
				Message = arguments.Message ?? SessionData.Login + " ha creado una nueva incidencia",
				Incidence = serviceIncidence,
				PhotoUrl = photoUrl,
				Longitude = arguments.Longitude,
				Latitude = arguments.Latitude
			};
			await ServiceNotificationRepository.AddAsync(serviceNotification);
			await UnitOfWork.SaveAsync();

			var receivers = new List<string>() {
				serviceNotification.ReceiverLogin
			};
			await PushService.SendNotification(receivers, serviceNotification.Type, serviceNotification.State, serviceNotification.Message, serviceNotification.ReferenceClass, serviceNotification.ReferenceId.ToString(), serviceNotification.Id);

			return serviceIncidence;
		}
		#endregion ExecuteAsync
	}
}
