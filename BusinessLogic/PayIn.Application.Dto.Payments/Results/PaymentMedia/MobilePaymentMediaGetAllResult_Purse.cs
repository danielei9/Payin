using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobilePaymentMediaGetAllResult_Purse
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Subtitle { get; set; }
		public string NumberHash { get; set; }
		public int? VisualOrder { get; set; }
		public int? ExpirationMonth { get; set; }
		public int? ExpirationYear { get; set; }
		public PaymentMediaState State { get; set; }
		public PaymentMediaType Type { get; set; }
		public string BankEntity { get; set; }
		public string Login { get; set; }
		public decimal Balance { get; set; }
		public string Image { get; set; }
	}
}
