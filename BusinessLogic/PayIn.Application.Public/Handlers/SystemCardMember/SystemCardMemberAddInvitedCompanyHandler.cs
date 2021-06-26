using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.SystemCardMember;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;


namespace PayIn.Application.Public.Handlers
{
	public class SystemCardMemberAddInvitedCompanyHandler : IServiceBaseHandler<SystemCardMemberAddInvitedCompanyArguments>
    {
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<SystemCardMember> SystemCardMemberRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceSupplier> ServiceSupplierRepository { get; set; }
        [Dependency] public IEntityRepository<ServiceConcession> ServiceConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
        
		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(SystemCardMemberAddInvitedCompanyArguments arguments)
		{

			//if (!arguments.AcceptTerms)
			//	throw new ApplicationException(string.Format(SecurityResources.AcceptTermsException));

			var now = DateTime.UtcNow;

            var payinPaymentConcessions = (await PaymentConcessionRepository.GetAsync())
                .Where(x =>
                    (x.Concession.State == ConcessionState.Active) &&
                    (x.Concession.Supplier.Login == "info@pay-in.es")
                );

            var supplier = (await ServiceSupplierRepository.GetAsync())
				.Where(x => x.Login == arguments.Login)
				.FirstOrDefault();
			if (supplier == null)
			{
				supplier = new ServiceSupplier
				{
					Login = arguments.Login,
					Name = arguments.Name,
					TaxName = arguments.TaxName,
					TaxNumber = arguments.TaxNumber,
					TaxAddress = arguments.TaxAddress
				};
				await ServiceSupplierRepository.AddAsync(supplier);
			}
			
			var serviceConcession = new ServiceConcession
			{
				Name = arguments.Name,
				Type = ServiceType.Charge,
				State = ConcessionState.Active
			};
			supplier.Concessions.Add(serviceConcession);

			//Space Remove from BankAccountNumber
			var bankAccountNumberCleaned = arguments.BankAccountNumber.Replace(" ", "");

			// Creating payment concession
			var paymentConcession = new PaymentConcession
			{
				Observations = arguments.Observations ?? "",
				Phone = arguments.Mobile, //arguments.Phone,
				BankAccountNumber = bankAccountNumberCleaned,
				Concession = serviceConcession,
				FormUrl = "",
				Address = arguments.Address,
				CreateConcessionDate = now.ToUTC(),
				OnPayedUrl = "",
				OnPayedEmail = "",
				OnPaymentMediaCreatedUrl = "",
				OnPaymentMediaErrorCreatedUrl = "",
				OnlineCartActivated = false, //arguments.OnlineCartActivated,
				CanHasPublicEvent = false,
				AllowUnsafePayments = false,
				Key = "",
				KeyType = null,
                LiquidationConcession = payinPaymentConcessions.FirstOrDefault()
            };
			await PaymentConcessionRepository.AddAsync(paymentConcession);

			// Activar el SystemCardMember
			var systemCardMember = (await SystemCardMemberRepository.GetAsync())
				.Where(x =>
					x.Login == arguments.Login
				)
				.FirstOrDefault();
			if (systemCardMember != null)
				systemCardMember.State = SystemCardMemberState.Active;

			return supplier;
		}
		#endregion ExecuteAsync
	}
}

