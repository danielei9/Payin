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
	public class PaymentConcessionPurseCreateHandler :
		IServiceBaseHandler<PaymentConcessionPurseCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<PaymentConcessionPurse> Repository;
		private readonly IEntityRepository<PaymentConcession> PaymentConcessionRepository;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;
		private readonly IUnitOfWork UnitOfWork;

		#region Constructors
		public PaymentConcessionPurseCreateHandler(
			ISessionData sessionData,
			IEntityRepository<PaymentConcessionPurse> repository,
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
		public async Task<dynamic> ExecuteAsync(PaymentConcessionPurseCreateArguments arguments)
		{
			//Ver si ya hemos añadido esa empresa
			var paymentConcession = (await Repository.GetAsync())
				.Where(x => 
					x.PaymentConcession.Concession.Supplier.Login == arguments.Login && 
					x.PurseId == arguments.Id
				)
				.FirstOrDefault();
			if (paymentConcession != null)
				throw new Exception(PaymentConcessionPurseResources.AddException);

			//Ver si existe la empresa
			var paymentConcessionFollow = (await PaymentConcessionRepository.GetAsync("Concession.Supplier"))
				.Where(x => x.Concession.Supplier.Login == arguments.Login)
				.FirstOrDefault();
			if (paymentConcessionFollow == null)
				throw new Exception(PaymentConcessionPurseResources.PaymentConcessionException);

			var paymentConcessionPurse = new PaymentConcessionPurse
			{
				State = PaymentConcessionPurseState.Pending,
				PaymentConcession = paymentConcessionFollow,
				PurseId = arguments.Id
			};
			await Repository.AddAsync(paymentConcessionPurse);
			await UnitOfWork.SaveAsync();

			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
					type: NotificationType.PaymentConcessionPurse,
					message: PaymentConcessionCampaignResources.AcceptAssociationMessage.FormatString(paymentConcessionFollow.Concession.Supplier.Name),
					referenceId: paymentConcessionPurse.Id,
					referenceClass: "PaymentConcessionPurse",
					senderLogin: SessionData.Login,
					receiverLogin: arguments.Login
			));

			return paymentConcessionPurse;
		}
		#endregion ExecuteAsync
	}
}

