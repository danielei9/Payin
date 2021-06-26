using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Internal.Arguments
{
	public class PaymentMediaCreateWebCardSabadellArguments : IArgumentsBase
	{
		public string  Version { get; set; }
		public decimal Amount { get; set; }
		public string Currency { get; set; }
		public int OrderId { get; set; }
		public string CommerceCode { get; set; }
		public int Terminal { get; set; }
		public string Signature { get; set; }
		public string Response { get; set; }
		public string ErrorCode { get; set; }
		public string ErrorText { get; set; }
		public string ErrorPublic { get; set; }
		public bool IsError { get; set; }
		public string TransactionType { get; set; }
		public bool SecurePayment { get; set; }
		public int PaymentMediaId { get; set; }
		public int PublicPaymentMediaId { get; set; }
		public int PublicTicketId { get; set; }
		public int PublicPaymentId { get; set; }
		public PaymentMediaCreateType PaymentMediaCreateType { get; set; }
		public string AuthorizationCode { get; set; }
		public int ExpirationMonth { get; set; }
		public int ExpirationYear { get; set; }
		public string CardIdentifier { get; set; }
		public string CardNumberHash { get; set; }

		#region Constructors
		public PaymentMediaCreateWebCardSabadellArguments(string version, decimal amount, string currency, int orderId, string commerceCode, int terminal, string signature, string response, string transactionType, bool securePayment, int paymentMediaId, int publicPaymentMediaId, PaymentMediaCreateType paymentMediaCreateType, int publicTicketId, int publicPaymentId, string authorizationCode, int expirationMonth, int expirationYear, string cardIdentifier, string cardNumberHash, bool isError)
		{
			Version = version;
			Amount = amount;
			Currency = currency;
			OrderId = orderId;
			CommerceCode = commerceCode;
			Terminal = terminal;
			Signature = signature;
			Response = response;
			TransactionType = transactionType;
			SecurePayment = securePayment;
			PaymentMediaId = paymentMediaId;
			PublicPaymentMediaId = publicPaymentMediaId;
			PublicTicketId = publicTicketId;
			PublicPaymentId = publicPaymentId;
			PaymentMediaCreateType = paymentMediaCreateType;
			AuthorizationCode = authorizationCode;
			ExpirationMonth = expirationMonth;
			ExpirationYear = expirationYear;
			CardIdentifier = cardIdentifier;
			CardNumberHash = cardNumberHash;
			IsError = isError;
		}
		#endregion Constructors
	}
}
