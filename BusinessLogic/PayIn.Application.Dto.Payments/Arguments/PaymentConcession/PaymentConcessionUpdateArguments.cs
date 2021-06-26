using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class PaymentConcessionUpdateArguments : IUpdateArgumentsBase<PayIn.Domain.Payments.PaymentConcession>
	{
																										public int Id { get; private set; }
		// Información comercial
		[Display(Name = "resources.paymentConcession.name")]                 [Required]					public string Name { get; private set; }
		[Display(Name = "resources.paymentConcession.phone")]                [Required]					public string Phone { get; private set; }
    																			       					public string Address { get; private set; }
		[Display(Name = "resources.paymentConcession.observations")]									public string Observations { get; private set; }



        [Display(Name = "resources.paymentConcession.state")]											public ConcessionState State { get; private set; }
		[Display(Name = "resources.paymentConcession.payinCommission")]	     [Required]					public decimal PayinCommission { get; private set; }
				    													      							public XpDate ConcessionCreateDate { get; private set; }
		[Display(Name = "resources.paymentConcession.liquidationAmountMin")] [Required]					public decimal LiquidationAmountMin { get; private set; }
		[Display(Name = "resources.paymentConcession.ticketTemplate")]									public int? TicketTemplateId { get; private set; }
		
		[Display(Name = "resources.paymentConcession.bannerPhotoUrl")] [DataType(DataType.ImageUrl)]	public string PhotoUrl { get; set; }


		[Display(Name = "resources.paymentConcession.cart")]											public bool OnlineCartActivated { get; set; }
		[Display(Name = "resources.paymentConcession.canHasPublicEvent")]								public bool CanHasPublicEvent { get; set; }
		[Display(Name = "resources.paymentConcession.allowUnsafePayments")]								public bool AllowUnsafePayments { get; set; }

		[Display(Name = "resources.paymentConcession.groupEntranceTypeByEvent")]						public bool GroupEntranceTypeByEvent { get; set; }
		[Display(Name = "resources.paymentConcession.onPayedEmail")]									public string OnPayedEmail { get; private set; }
		[Display(Name = "resources.paymentConcession.onPaymentMediaCreatedUrl")]						public string OnPaymentMediaCreatedUrl { get; private set; }
		[Display(Name = "resources.paymentConcession.onPaymentMediaErrorCreatedUrl")]					public string OnPaymentMediaErrorCreatedUrl { get; private set; }

		#region Constructors
		public PaymentConcessionUpdateArguments(int id, string name, ConcessionState state, 
            decimal payinCommission,decimal liquidationAmountMin, XpDate concessionCreateDate,
            string address, string phone, string observations, int? ticketTemplateId, bool onlinecartActivated,
			bool canHasPublicEvent, bool allowUnsafePayments, 
			string onPayedEmail, string onPaymentMediaCreatedUrl, string onPaymentMediaErrorCreatedUrl, bool groupEntranceTypeByEvent)
		{
			Id = id;
			Name = name;		
			State = state;
			PayinCommission = payinCommission;
			ConcessionCreateDate = concessionCreateDate;
			LiquidationAmountMin = liquidationAmountMin;
			Address = address;
			Phone = phone;			
			Observations = observations;
			TicketTemplateId = ticketTemplateId;
            OnlineCartActivated = onlinecartActivated;
			CanHasPublicEvent = canHasPublicEvent;
			AllowUnsafePayments = allowUnsafePayments;
			OnPayedEmail = onPayedEmail;
			OnPaymentMediaCreatedUrl = onPaymentMediaCreatedUrl;
			OnPaymentMediaErrorCreatedUrl = onPaymentMediaErrorCreatedUrl;
			GroupEntranceTypeByEvent = groupEntranceTypeByEvent;
		}
		#endregion Constructors
	}
}
