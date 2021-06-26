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
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentConcessionCampaignMobileAcceptAssignmentHandler : IServiceBaseHandler<PaymentConcessionCampaignMobileAcceptAssignmentArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<PaymentConcessionCampaign> Repository;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public PaymentConcessionCampaignMobileAcceptAssignmentHandler(
			ISessionData sessionData,
			IEntityRepository<PaymentConcessionCampaign> repository,
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
		public async Task<dynamic> ExecuteAsync(PaymentConcessionCampaignMobileAcceptAssignmentArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					Name = x.PaymentConcession.Concession.Supplier.Name,
					CampaignName = x.Campaign.Title,
					CampaignLogin = x.Campaign.Concession.Concession.Supplier.Login,
					Item = x
				})
				.FirstOrDefault();

			item.Item.State = PaymentConcessionCampaignState.Active;

			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
				type: NotificationType.ConcessionVinculationAccepted,
				message: PaymentConcessionCampaignResources.AssociationAccepted.FormatString(item.Name, item.CampaignName),
				referenceId: arguments.Id,
				referenceClass: "PaymentConcessionCampaign",
				senderLogin: SessionData.Login,
				receiverLogin: item.CampaignLogin
			));
			return item.Item;
		}
		#endregion ExecuteAsync
	}
}