using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Public.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentWorkerMobileAcceptAssignmentHandler : IServiceBaseHandler<PaymentWorkerMobileAcceptAssignmentArguments>
	{
		private readonly IEntityRepository<PaymentWorker> Repository;
		private readonly ISessionData SessionData;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public PaymentWorkerMobileAcceptAssignmentHandler(
			ISessionData sessionData,
			IEntityRepository<PaymentWorker> repository,
			ServiceNotificationCreateHandler serviceNotificationCreate
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;

			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;

			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");
			ServiceNotificationCreate = serviceNotificationCreate;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PaymentWorkerMobileAcceptAssignmentArguments arguments)
		{
			var item = (await Repository.GetAsync("Concession.Concession.Supplier"))
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.State = WorkerState.Active;

			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
				type: NotificationType.ConcessionVinculationAccepted,
				message: PaymentWorkerResources.AssociationAccepted.FormatString(item.Name, item.Concession.Concession.Supplier.Name),
				referenceId: arguments.Id,
				referenceClass: "PaymentWorker",
				senderLogin: item.Login,
				receiverLogin: item.Concession.Concession.Supplier.Login
			));

			var securityRepository = new SecurityRepository();
			await securityRepository.AddRole(item.Login, AccountRoles.PaymentWorker);

			return item;
		}
		#endregion ExecuteAsync
	}
}