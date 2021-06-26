using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class PaymentConcessionPurseDeleteHandler :
		IServiceBaseHandler<PaymentConcessionPurseDeleteArguments>
	{
		private readonly IEntityRepository<PaymentConcessionPurse> Repository;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;
		private readonly IEntityRepository<CampaignLine> CampaignLineRepository;
		private readonly ISessionData SessionData;


		#region Constructors
		public PaymentConcessionPurseDeleteHandler(ISessionData sessionData, IEntityRepository<PaymentConcessionPurse> repository, IEntityRepository<CampaignLine> campaignLineRepository, ServiceNotificationCreateHandler serviceNotificationCreate)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (campaignLineRepository == null) throw new ArgumentNullException("campaignLineRepository");
			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");

			SessionData = sessionData;
			Repository = repository;
			CampaignLineRepository = campaignLineRepository;
			ServiceNotificationCreate = serviceNotificationCreate;
		}
		#endregion Constructors

		#region PaymentConcessionPurseDelete
		public async Task<dynamic> ExecuteAsync(PaymentConcessionPurseDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync("PaymentConcession.Concession.Supplier","Purse"))
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			var campaignLines = (await CampaignLineRepository.GetAsync())
				.Where(x => x.PurseId == item.Purse.Id && x.State == CampaignLineState.Active);

			if (campaignLines.Count() > 0)
				throw new Exception(PaymentConcessionPurseResources.PurseWithCampaignLineError);		

			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
				type: NotificationType.ConcessionDissociation,
				message: PaymentConcessionPurseResources.DissociationMessage.FormatString(item.Purse.Name),
				referenceId: item.Id,
				referenceClass: "PaymentConcessionPurse",
				senderLogin: SessionData.Login,
				receiverLogin: item.PaymentConcession.Concession.Supplier.Login
				));
			item.State = PaymentConcessionPurseState.Deleted;

			return null;
		}
		#endregion PaymentUserDelete
	}
}
