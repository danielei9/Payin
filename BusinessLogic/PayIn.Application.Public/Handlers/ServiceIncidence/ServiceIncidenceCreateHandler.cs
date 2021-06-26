using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceIncidenceCreateHandler :
		IServiceBaseHandler<ServiceIncidenceCreateArguments>
	{
		[Dependency] public IEntityRepository<ServiceIncidence> ServiceIncidenceRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceNotification> ServiceNotificationRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceUser> ServiceUserRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceConcession> ServiceConcessionRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceIncidenceCreateArguments arguments)
		{
			var serviceConcession = (await ServiceConcessionRepository.GetAsync(arguments.ConcessionId, "Supplier"));
			if (serviceConcession == null)
				throw new ArgumentException("ConcessionId");

			var serviceUser = (await ServiceUserRepository.GetAsync())
				.Where(x => x.Login == SessionData.Login)
				.FirstOrDefault();
			if (serviceUser == null)
				throw new ArgumentException("ServiceUserId");

			var serviceIncidence = new ServiceIncidence
			{
				Concession = serviceConcession,
				Name = arguments.Name,
				Type = arguments.Type,
				Category = arguments.Category,
				SubCategory = arguments.SubCategory,
				DateTime = XpDateTime.FromDateTime(DateTime.Now),
				State = IncidenceState.Active//,
				//PhotoUrl = arguments.PhotoUrl,
				//Longitude = arguments.Longitude,
				//Latitude = arguments.Latitude,
				//UserLogin = SessionData.Login,
				//UserName = serviceUser.Name,
				//UserName = serviceUser.Phone//,
				////Observations = ""
			};
			await ServiceIncidenceRepository.AddAsync(serviceIncidence);

			var serviceNotification = new ServiceNotification
			{
				Type = NotificationType.ServiceNotificationCreate,
				State = NotificationState.Actived,
				//ReferenceId = null,
				ReferenceClass = "",
				SenderLogin = SessionData.Login,
				ReceiverLogin = serviceConcession.Supplier.Login,
				CreatedAt = DateTime.Now,
				IsRead = false,
				Message = String.Format("Tipo '{0}', Categoría '{1}', Subcategoría '{2}': {3}", serviceIncidence.Type.ToEnumAlias(), serviceIncidence.Category.ToEnumAlias(), serviceIncidence.SubCategory.ToEnumAlias(), serviceIncidence.Name),
				Incidence = serviceIncidence
			};
			await ServiceNotificationRepository.AddAsync(serviceNotification);

			return serviceIncidence;
		}
		#endregion ExecuteAsync
	}
}
