using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Public.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("PaymentConcessionPurse", "ResendNotification")]
	public class PaymentConcessionPurseResendNotificationHandler : IServiceBaseHandler<PaymentConcessionPurseResendNotificationArguments>
	{
		private readonly IEntityRepository<PaymentConcessionPurse> Repository;
		private readonly ISessionData SessionData;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public PaymentConcessionPurseResendNotificationHandler(
			ISessionData sessionData,
			IEntityRepository<PaymentConcessionPurse> repository,
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
		public async Task<dynamic> ExecuteAsync(PaymentConcessionPurseResendNotificationArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id, "PaymentConcession.Concession.Supplier");
			if (item == null)
				throw new ArgumentNullException("id");

			item.State = PaymentConcessionPurseState.Pending;

			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
				type: NotificationType.PaymentConcessionPurse,
				message: PaymentConcessionPurseResources.AcceptAssociationMessage.FormatString(item.PaymentConcession.Concession.Supplier.Name),
				referenceId: item.Id,
				referenceClass: "PaymentConcessionPurse",
				senderLogin: SessionData.Login,
				receiverLogin: item.PaymentConcession.Concession.Supplier.Login
			));
			return item;
		}
		#endregion ExecuteAsync
	}
}