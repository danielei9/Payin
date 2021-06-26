using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{

	public partial class PaymentConcessionGetResult
	{
		public int Id { get; set; }
		// Información comercial
		public string Name { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public string Observations { get; set; }
		//
		public ConcessionState State { get; set; }
		public ServiceType Type { get; set; }	
		public decimal PayinCommission { get; set; }
		public decimal LiquidationAmountMin { get; set; }
		public int? TicketTemplateId { get; set; }
		public string TicketTemplateName { get; set; }
        public bool OnlineCartActivated { get; set; }
		public bool CanHasPublicEvent { get; set; }
		public bool AllowUnsafePayments { get; set; }
		public string OnPayedEmail { get; set; }
		public string OnPaymentMediaCreatedUrl { get; set; }
		public string OnPaymentMediaErrorCreatedUrl { get; set; }
		public bool GroupEntranceTypeByEvent { get; set; }

		public string PhotoUrl { get; set; }
	}
}