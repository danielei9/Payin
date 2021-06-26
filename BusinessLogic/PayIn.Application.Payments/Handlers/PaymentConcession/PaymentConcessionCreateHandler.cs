using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Common.Security;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("PaymentConcession", "Create")]
	[XpAnalytics("PaymentConcession", "Create")]
	public class PaymentConcessionCreateHandler : IServiceBaseHandler<PaymentConcessionCreateArguments>
	{
		private ISessionData SessionData { get; set; }
		private IEntityRepository<EntranceSystem> RepositoryEntranceSystem { get; set; }
		private IEntityRepository<ServiceSupplier> RepositoryServiceSupplier { get; set; }
		private IEntityRepository<PaymentConcession> Repository { get; set; }
		private IUnitOfWork UnitOfWork;
		private IInternalService InternalService;
			
		#region Construtors
		public PaymentConcessionCreateHandler
		(
			ISessionData sessionData,
			IEntityRepository<EntranceSystem> repositoryEntranceSystem,
			IEntityRepository<ServiceSupplier> repositoryServiceSupplier,
			IEntityRepository<PaymentConcession>repository,
			IUnitOfWork unitOfWork,
			IInternalService internalService
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repositoryEntranceSystem == null) throw new ArgumentNullException("repositoryEntranceSystem");
			if (repositoryServiceSupplier == null) throw new ArgumentNullException("repositoryServiceSupplier");
			if (repository == null) throw new ArgumentNullException("repository");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (internalService == null) throw new ArgumentNullException("internalService");

			SessionData = sessionData;
			RepositoryEntranceSystem = repositoryEntranceSystem;
			RepositoryServiceSupplier = repositoryServiceSupplier;
			Repository = repository;
			UnitOfWork = unitOfWork;
			InternalService = internalService;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PaymentConcessionCreateArguments arguments)
		{
			if (!arguments.AcceptTerms)
				throw new ApplicationException(string.Format(SecurityResources.AcceptTermsException));

			var now = DateTime.UtcNow;

            var payinPaymentConcessions = (await Repository.GetAsync())
                .Where(x =>
                    (x.Concession.State == ConcessionState.Active) &&
                    (x.Concession.Supplier.Login == "info@pay-in.es")
                );

			var supplier = (await RepositoryServiceSupplier.GetAsync("Concessions"))
				.Where(x => x.Login == SessionData.Login)
				.FirstOrDefault();
			if (supplier == null)
			{
				supplier = new ServiceSupplier
				{
					Login = SessionData.Login,
					Name = arguments.Name,
					TaxName = arguments.TaxName,
					TaxNumber = arguments.TaxNumber,
					TaxAddress = arguments.TaxAddress
				};
				await RepositoryServiceSupplier.AddAsync(supplier);
			}

			var serviceConcession = new ServiceConcession
			{
				Name = arguments.Name,
				Type = ServiceType.Charge,
				State = ConcessionState.Pending
			};
			supplier.Concessions.Add(serviceConcession);
			
			//Space Remove from BankAccountNumber
			var bankAccountNumberCleaned = arguments.BankAccountNumber.Replace(" ","");

			// Creating payment concession
			var paymentConcession = new PaymentConcession
			{
				Observations = arguments.Observations ?? "",
				Phone = arguments.Phone,
				BankAccountNumber = bankAccountNumberCleaned,
				Concession = serviceConcession,
				FormUrl = "",
				Address = arguments.Address,
				CreateConcessionDate = now.ToUTC(),
				OnPayedUrl = "",
				OnPayedEmail = "",
				OnPaymentMediaCreatedUrl = "",
				OnPaymentMediaErrorCreatedUrl = "",
                OnlineCartActivated = arguments.OnlineCartActivated,
				CanHasPublicEvent = false,
				AllowUnsafePayments = false,
				Key = "",
				KeyType = null,
                LiquidationConcession = payinPaymentConcessions.FirstOrDefault()
            };
			await Repository.AddAsync(paymentConcession);

			// Add default entrance systems to payment concession
			var entranceSystems = (await RepositoryEntranceSystem.GetAsync())
				.Where(x => x.IsDefault);
			foreach (var entranceSystem in entranceSystems)
				paymentConcession.EntranceSystems.Add(entranceSystem);

			// Create internal user to charge
            if (!(await InternalService.UserHasPaymentAsync()))
            {
                await InternalService.UserCreateAsync(arguments.Pin);

                var securityRepository = new SecurityRepository();
                await securityRepository.UpdateTaxDataAsync(
                    SessionData.Login,
                    arguments.TaxNumber,
                    arguments.TaxName,
                    arguments.TaxAddress,
                    null
                );
            }
			await UnitOfWork.SaveAsync();
	
			//Save file in Azure
			var repositoryAzure = new AzureBlobRepository();			
			var guid = Guid.NewGuid();
			
			paymentConcession.FormUrl = repositoryAzure.SaveFile(PaymentConcessionResources.FileShortUrl.FormatString(paymentConcession.Id, guid), arguments.FormA);
			
			return paymentConcession;
		}
		#endregion ExecuteAsync
	}
}
