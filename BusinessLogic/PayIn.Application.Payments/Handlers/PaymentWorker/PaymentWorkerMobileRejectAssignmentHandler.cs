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
	public class PaymentWorkerMobileRejectAssignmentHandler : IServiceBaseHandler<PaymentWorkerMobileRejectAssignmentArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<PaymentWorker> Repository;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public PaymentWorkerMobileRejectAssignmentHandler(
			ISessionData sessionData,
			IEntityRepository<PaymentWorker> repository,
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
		public async Task<dynamic> ExecuteAsync(PaymentWorkerMobileRejectAssignmentArguments arguments)
		{
			var item = (await Repository.GetAsync("Concession.Concession.Supplier"))
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.State = WorkerState.Unsuscribed;

			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
				type: NotificationType.ConcessionVinculationRefused,
				message: PaymentWorkerResources.AssociationRefused.FormatString(item.Name, item.Concession.Concession.Supplier.Name),
				referenceId: arguments.Id,
				referenceClass: "PaymentWorker",
				senderLogin: item.Login,
				receiverLogin: item.Concession.Concession.Supplier.Login
			));

			var securityRepository = new SecurityRepository();
			await securityRepository.RemoveRole(item.Login, AccountRoles.PaymentWorker);

			return item;
		}
		#endregion ExecuteAsync
	}
}