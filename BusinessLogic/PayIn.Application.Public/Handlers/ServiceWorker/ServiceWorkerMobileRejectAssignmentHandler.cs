using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Arguments.ServiceWorker;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceWorkerMobileRejectAssignmentHandler :
		IServiceBaseHandler<ServiceWorkerMobileRejectAssignmentArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<ServiceWorker> Repository;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public ServiceWorkerMobileRejectAssignmentHandler(
			ISessionData sessionData,
			IEntityRepository<ServiceWorker> repository,
			ServiceNotificationCreateHandler serviceNotificationCreate
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");

			SessionData = sessionData;
			Repository = repository;
			ServiceNotificationCreate = serviceNotificationCreate;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceWorkerMobileRejectAssignmentArguments>.ExecuteAsync(ServiceWorkerMobileRejectAssignmentArguments arguments)
		{
			var worker = (await Repository.GetAsync("Supplier"))
				.Where(x => x.Id == arguments.Id && x.Login == SessionData.Login)
				.FirstOrDefault();

			if (worker == null) throw new ArgumentNullException("worker");

			worker.State = WorkerState.Deleted;

			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
				type: NotificationType.ConcessionVinculationRefused,
				message: ServiceWorkerResources.AssociationRefused.FormatString(worker.Name,worker.Supplier.Name),
				referenceId: arguments.Id,
				referenceClass: "ServiceWorker",
				senderLogin: SessionData.Login,
				receiverLogin: worker.Supplier.Login
			));
			return worker;
		}
		#endregion ExecuteAsync
	}
}

