using PayIn.Application.Dto.Payments.Arguments.Purse;
using PayIn.Application.Dto.Arguments.Notification;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Common.Resources;
using PayIn.Common;
using Xp.Common.Exceptions;
using System.Collections.Generic;

namespace PayIn.Application.Public.Handlers
{
	public class PurseCreateHandler :
		IServiceBaseHandler<PurseCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<Purse> Repository;
		private readonly IEntityRepository<PaymentMedia> PaymentMediaRepository;
		private readonly IEntityRepository<PaymentConcession> ConcessionRepository;
		private readonly IEntityRepository<PaymentConcessionPurse> PaymentRepository;
		private readonly IUnitOfWork UnitOfWork;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public PurseCreateHandler(
			ISessionData sessionData,
			IEntityRepository<Purse> repository,
			IEntityRepository<PaymentMedia> paymentMediaRepository,
            IEntityRepository<PaymentConcession> concessionRepository,
			IEntityRepository<PaymentConcessionPurse> paymentRepository,
			IUnitOfWork unitOfWork,
			ServiceNotificationCreateHandler serviceNotificationCreate
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (paymentMediaRepository == null) throw new ArgumentNullException("paymentMediaRepository");
			if (concessionRepository == null) throw new ArgumentNullException("paymentConcession");
			if (paymentRepository == null) throw new ArgumentNullException("paymentConcessionCampaign");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");
			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");
			SessionData = sessionData;
			Repository = repository;
			PaymentMediaRepository = paymentMediaRepository;
			ConcessionRepository = concessionRepository;
			PaymentRepository = paymentRepository;
			UnitOfWork = unitOfWork;
			ServiceNotificationCreate = serviceNotificationCreate;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PurseCreateArguments arguments)
		{
			var now = DateTime.Now.ToUTC();

			if (arguments.Validity <= now)
				throw new Exception(PurseResources.ValidityException);
			if (arguments.Expiration <= now)
				throw new Exception(PurseResources.ExpiratedException);
			
			var concessions = await ConcessionRepository.GetAsync("Concession.Supplier");
			var concession = concessions
				.Where(x => x.Concession.Supplier.Login == SessionData.Login)
				.FirstOrDefault();
			
			var purse = new Purse
			{
				Name = arguments.Name,				
				Expiration = arguments.Expiration.Value.ToUTC(),
				Validity = arguments.Validity.Value.ToUTC(),				
				Concession = concession,
				State = PurseState.Active
			};
			await Repository.AddAsync(purse);

			var paymentMedia = new PaymentMedia
			{
				Name = arguments.Name,
				NumberHash = "",
				ExpirationMonth = arguments.Expiration.Value.Month,
				ExpirationYear = arguments.Expiration.Value.Year,
				VisualOrder = null,
				VisualOrderFavorite = null,
				Login = SessionData.Login,
				State = PaymentMediaState.Active,
				BankEntity = "",
				Type = PaymentMediaType.Purse,
				Purse = purse,
				UserName = SessionData.Name,
				UserLastName = SessionData.TaxName,
				UserBirthday = null,
				UserTaxNumber = SessionData.TaxNumber,
				UserAddress = SessionData.TaxAddress,
				UserPhone = "",
				UserEmail = SessionData.Email,
				Default = false
			};
			await PaymentMediaRepository.AddAsync(paymentMedia);

			var vinculate = new PaymentConcessionPurse
			{
				State = PaymentConcessionPurseState.Active,
				PaymentConcession = concession,
				Purse = purse
			};
			purse.PaymentConcessionPurses.Add(vinculate);
			
			return purse;
		}
		#endregion ExecuteAsync
	}
}

