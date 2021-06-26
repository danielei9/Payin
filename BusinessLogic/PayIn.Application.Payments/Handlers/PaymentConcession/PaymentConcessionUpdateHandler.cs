using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	[XpLog("PaymentConcession", "Update")]
	public class PaymentConcessionUpdateHandler :
		IServiceBaseHandler<PaymentConcessionUpdateArguments>
	{
		[Dependency] public IEntityRepository<PaymentConcession> Repository { get; set; }
		
		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PaymentConcessionUpdateArguments arguments)
		{
			var securityRepository = new SecurityRepository();

			var paymentConcessions = (await Repository.GetAsync("Concession.Supplier"));
			var paymentConcession = paymentConcessions
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			var activeConcessions = paymentConcessions
				.Where(x => x.Concession.Supplier.Login == paymentConcession.Concession.Supplier.Login && x.Concession.State == ConcessionState.Active)
				.Count();			

			var previousState = paymentConcession.Concession.State;

			if (paymentConcession != null)
			{				
				paymentConcession.PayinCommision = arguments.PayinCommission;
				paymentConcession.LiquidationAmountMin = arguments.LiquidationAmountMin;				
				paymentConcession.Concession.Name = arguments.Name;			
				paymentConcession.Concession.State = arguments.State;
				paymentConcession.TicketTemplateId = arguments.TicketTemplateId;
                paymentConcession.OnlineCartActivated = arguments.OnlineCartActivated;
				paymentConcession.CanHasPublicEvent = arguments.CanHasPublicEvent;
				paymentConcession.AllowUnsafePayments = arguments.AllowUnsafePayments;
				paymentConcession.GroupEntranceTypeByEvent = arguments.GroupEntranceTypeByEvent;
				paymentConcession.OnPayedEmail = arguments.OnPayedEmail;
				paymentConcession.OnPaymentMediaCreatedUrl = arguments.OnPaymentMediaCreatedUrl;
				paymentConcession.OnPaymentMediaErrorCreatedUrl = arguments.OnPaymentMediaErrorCreatedUrl;
			}
			if (arguments.State == ConcessionState.Active && activeConcessions == 0)
				await securityRepository.AddRole(paymentConcession.Concession.Supplier.Login, AccountRoles.CommercePayment);
			return paymentConcession;
		}
		#endregion ExecuteAsync
	}
}

