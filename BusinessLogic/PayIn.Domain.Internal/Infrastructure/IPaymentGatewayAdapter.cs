using PayIn.Common;
using System.Threading.Tasks;

namespace PayIn.Domain.Internal.Infrastructure
{
	public interface IPaymentGatewayAdapter
	{
		Task<string> WebCardRequestAsync(string commerceCode, int? paymentMediaId, int? publicPaymentMediaId, int publicTicketId, int publicPaymentId, int order, PaymentMediaCreateType paymentMediaCreateType, decimal amount);
		Task<string> PayAsync(int paymentMediaId, int publicPaymentMediaId, int publicTicketId, int publicPaymentId, string cardIdentifier, int order, decimal amount);
		Task<string> RefundAsync(string commerceCode, int? paymentMediaId, int? publicPaymentMediaId, int publicTicketId, int publicPaymentId, string cardIdentifier, int order, decimal amount);
		bool VerifyResponse(string data, string signature);
		bool VerifyResponse(decimal amount, int orderId, string commerceCode, string currency, string response, string cardNumberHash, string transactionType, bool securePayment, string signature);
		bool VerifyCommerceCode(string commerceCode);
	}
}
