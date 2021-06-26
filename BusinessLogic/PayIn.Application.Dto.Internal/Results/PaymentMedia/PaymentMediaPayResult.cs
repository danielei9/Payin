namespace PayIn.Application.Dto.Internal.Results
{
	public class PaymentMediaPayResult
	{
		public decimal Amount { get; set; }
		public string Currency { get; set; }
		public int OrderId { get; set; }
		public string Signature { get; set; }
		public string CommerceCode { get; set; }
		public int Terminal { get; set; }
		public string Response { get; set; }
		public string Code { get; set; }
		public string ErrorCode { get; set; }
		public string ErrorText { get; set; }
		public string ErrorPublic { get; set; }
		public bool IsError { get; set; }
		public string AuthorizationCode { get; set; }
		public string TransactionType { get; set; }
		public bool SecurePayment { get; set; }
		public string Language { get; set; }
		public string CardIdentifier { get; set; }
		public PaymentMediaPayResult_Data MerchantData { get; set; }
		public string CardCountry { get; set; }
	}
}
