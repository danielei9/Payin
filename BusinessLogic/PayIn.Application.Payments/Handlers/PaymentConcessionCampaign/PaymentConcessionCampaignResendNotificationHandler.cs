using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Public.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("PaymentConcessionCampaign", "ResendNotification")]
	public class PaymentConcessionCampaignResendNotificationHandler : IServiceBaseHandler<PaymentConcessionCampaignResendNotificationArguments>
	{
		private readonly IEntityRepository<PaymentConcessionCampaign> Repository;
		private readonly ISessionData SessionData;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public PaymentConcessionCampaignResendNotificationHandler(
			ISessionData sessionData,
			IEntityRepository<PaymentConcessionCampaign> repository,
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
		public async Task<dynamic> ExecuteAsync(PaymentConcessionCampaignResendNotificationArguments arguments)
		{
			var item = (await Repository.GetAsync("PaymentConcession.Concession.Supplier"))
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.State = PaymentConcessionCampaignState.Pending;

			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
				type: NotificationType.PaymentConcessionCampaign,
				message: PaymentConcessionCampaignResources.AcceptAssociationMessage.FormatString(item.PaymentConcession.Concession.Supplier.Name),
				referenceId: item.Id,
				referenceClass: "PaymentConcessionCampaign",
				senderLogin: SessionData.Login,
				receiverLogin: item.PaymentConcession.Concession.Supplier.Login
			));
			return item;
		}
		#endregion ExecuteAsync
	}
}