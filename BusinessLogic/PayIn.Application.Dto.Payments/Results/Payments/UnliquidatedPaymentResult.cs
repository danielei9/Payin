using PayIn.Common;
using System;

namespace PayIn.Application.Dto.Payments.Results.Payment
{
	public class UnliquidatedPaymentResult
	{
		public int Id { get; set; }
		public decimal Amount { get; set; }
		public DateTime Date { get; set; }
		public string Name { get; set; }
		public string PaymentMedia { get; set; }
		public string TaxName { get; set; }
		public string SupplierName { get; set; }
		public string SupplierAddress { get; set; }
		public string SupplierTaxNumber { get; set; }
		public decimal PayinCommission { get; set; }
		//public string TaxAddress { get; set; }
		public string UserLogin { get; set; }
		public string UserName { get; set; }
		public decimal TicketAmount { get; set; }
		public string Ticket { get; set; }
		public string NumberHash { get; set; }
		public PaymentMediaType CardType { get; set; }
		public decimal TotalWithoutPayin { get; set; }
		public PaymentState State { get; set; }
		public string StateName { get; set; }
		public string fotoUrl { get; set; }
		public string SupplierLogin { get; set; }
		public int? RefundFromId { get; set; }
		public DateTime? RefundFromDate { get; set; }
		public int? RefundToId { get; set; }
		public DateTime? RefundToDate { get; set; }
		public bool FotoExists { get; set; }

	}
}
