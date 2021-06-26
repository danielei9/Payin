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
	public class PaymentConcessionCampaignCreateHandler :
		IServiceBaseHandler<PaymentConcessionCampaignCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<PaymentConcessionCampaign> Repository;
		private readonly IEntityRepository<PaymentConcession> PaymentConcessionRepository;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;
		private readonly IUnitOfWork UnitOfWork;

		#region Constructors
		public PaymentConcessionCampaignCreateHandler(
			ISessionData sessionData,
			IEntityRepository<PaymentConcessionCampaign> repository,
			IEntityRepository<PaymentConcession> paymentConcessionRepository,
			ServiceNotificationCreateHandler serviceNotificationCreate,
			IUnitOfWork unitOfWork
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (paymentConcessionRepository == null) throw new ArgumentNullException("repository");
			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate"); 
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");



			SessionData = sessionData;
			Repository = repository;
			PaymentConcessionRepository = paymentConcessionRepository;
			ServiceNotificationCreate = serviceNotificationCreate;
			UnitOfWork = unitOfWork;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<PaymentConcessionCampaignCreateArguments>.ExecuteAsync(PaymentConcessionCampaignCreateArguments arguments)
		{
			//Ver si ya hemos añadido esa empresa
			var paymentConcession = (await Repository.GetAsync("PaymentConcession.Concession.Supplier"))
			.Where(x => x.PaymentConcession.Concession.Supplier.Login == arguments.Login && x.CampaignId == arguments.Id).FirstOrDefault();

			//Ver si existe la empresa
			var paymentConcessionFollow = (await PaymentConcessionRepository.GetAsync("Concession.Supplier"))
			.Where(x => x.Concession.Supplier.Login == arguments.Login).FirstOrDefault();
			
			if (paymentConcession != null)
			{
				throw new Exception(PaymentConcessionCampaignResources.AddException);
			}

			if (paymentConcessionFollow == null)
			{
				throw new Exception(PaymentConcessionCampaignResources.PaymentConcessionException);
			}


			var paymentConcessionCampaign = new PaymentConcessionCampaign
			{
				State = PaymentConcessionCampaignState.Pending,
				PaymentConcession =  paymentConcessionFollow,
				CampaignId = arguments.Id
				
			};
			await Repository.AddAsync(paymentConcessionCampaign);
			await UnitOfWork.SaveAsync();


			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
					type: NotificationType.PaymentConcessionCampaign,
					message: PaymentConcessionCampaignResources.AcceptAssociationMessage.FormatString(paymentConcessionFollow.Concession.Supplier.Name),
					referenceId: paymentConcessionCampaign.Id,
					referenceClass: "PaymentConcessionCampaign",
					senderLogin: SessionData.Login,
					receiverLogin: arguments.Login
			));

			return paymentConcessionCampaign;
		}
		#endregion ExecuteAsync
	}
}

