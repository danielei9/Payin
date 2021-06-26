using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Arguments.ServiceWorker;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceWorkerMobileAcceptAssignmentHandler :
		IServiceBaseHandler<ServiceWorkerMobileAcceptAssignmentArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<ServiceWorker> Repository;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public ServiceWorkerMobileAcceptAssignmentHandler(
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
		async Task<dynamic> IServiceBaseHandler<ServiceWorkerMobileAcceptAssignmentArguments>.ExecuteAsync(ServiceWorkerMobileAcceptAssignmentArguments arguments)
		{
			var worker = (await Repository.GetAsync("Supplier"))
				.Where(x => x.Id == arguments.Id && x.Login == SessionData.Login)
				.FirstOrDefault();

			if (worker == null) throw new ArgumentNullException("worker");

			worker.State = WorkerState.Active;

			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
				type: NotificationType.ConcessionVinculationAccepted,
				message: ServiceWorkerResources.AssociationAccepted.FormatString(worker.Name, worker.Supplier.Name),
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

