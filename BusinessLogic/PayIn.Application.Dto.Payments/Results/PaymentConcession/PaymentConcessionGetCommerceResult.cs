using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PaymentConcessionGetCommerceResult
	{
		public int Id { get; set; }

		public string TaxNumber { get; set; }
		public string TaxName { get; set; }
		public string TaxAddress { get; set; }
		public string BankAccountNumber { get; set; }
		public string FormUrl { get; set; }

		public string Name { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public string Observations { get; set; }
		public decimal PayinCommission { get; set; }
		public decimal LiquidationAmountMin { get; set; }
		public ConcessionState State { get; set; }
	}
}
