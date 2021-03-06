using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Public.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("PaymentWorker", "ResendNotification")]
	public class PaymentWorkerResendNotificationHandler : IServiceBaseHandler<PaymentWorkerResendNotificationArguments>
	{
		private readonly IEntityRepository<PaymentWorker> Repository;
		private readonly ISessionData SessionData;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public PaymentWorkerResendNotificationHandler(
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
		public async Task<dynamic> ExecuteAsync(PaymentWorkerResendNotificationArguments arguments)
		{
			var item = (await Repository.GetAsync("Concession.Concession.Supplier"))
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.State = WorkerState.Pending;

			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
				type: NotificationType.ConcessionVinculation,
				message: PaymentWorkerResources.AcceptAssociationMessage.FormatString(item.Concession.Concession.Supplier.Name),
				referenceId: item.Id,
				referenceClass: "PaymentWorker",
				senderLogin: SessionData.Login,
				receiverLogin: item.Login
			));
			return item;
		}
		#endregion ExecuteAsync
	}
}