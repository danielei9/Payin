using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Public.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentConcessionCampaignDeleteHandlercs :
		IServiceBaseHandler<PaymentConcessionCampaignDeleteArguments>
	{
		private readonly IEntityRepository<PaymentConcessionCampaign> Repository;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;
		private readonly ISessionData SessionData;

		#region Constructors
		public PaymentConcessionCampaignDeleteHandlercs(ISessionData sessionData, IEntityRepository<PaymentConcessionCampaign> repository, ServiceNotificationCreateHandler serviceNotificationCreate)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			Repository = repository;
			ServiceNotificationCreate = serviceNotificationCreate;
			SessionData = sessionData;

		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<PaymentConcessionCampaignDeleteArguments>.ExecuteAsync(PaymentConcessionCampaignDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync("PaymentConcession.Concession.Supplier", "Campaign"))
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
				type: NotificationType.ConcessionDissociation,
				message: PaymentConcessionCampaignResources.DissociationMessage.FormatString(item.Campaign.Title),
				referenceId: item.Id,
				referenceClass: "PaymentConcessionCampaign",
				senderLogin: SessionData.Login,
				receiverLogin: item.PaymentConcession.Concession.Supplier.Login
				));

			item.State = PaymentConcessionCampaignState.Deleted;

			return null;
		}
		#endregion ExecuteAsync
	}
}
