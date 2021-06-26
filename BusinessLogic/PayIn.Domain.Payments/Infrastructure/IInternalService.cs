using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Application.Dto.Internal.Results;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Common;
using System.Threading.Tasks;

namespace PayIn.Domain.Payments.Infrastructure
{
	public interface IInternalService
	{
        // Version
        Task<MainGetVersionResult> VersionAsync();

        // User
        Task UserCreateAsync(string pin);
		Task<bool> UserHasPaymentAsync();
		Task UserUpdatePinAsync(string oldPin, string pin, string confirmPin);
        Task<UserForgotPinResult> UserForgotPinAsync(string pin);
        Task<bool> UserCheckPinAsync(string pin);

        // Ticket
        Task<PaymentMediaCreateWebCardResult> TicketPayWeb(
			string commerceCode,
			int publicTicketId, int publicPaymentId,
            string login, int orderId, PaymentMediaCreateType paymentMediaCreateType, decimal amount,
            string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile
        );

        // Payments
        Task<PaymentMediaCreateWebCardResult> PaymentMediaCreateWebCardAsync(
			string commerceCode,
			string pin, string name, int orderId, int publicPaymentMediaId, int publicTicketId, int publicPaymentId, string bankEntity, 
			string login, PaymentMediaCreateType paymentMediaCreateType, decimal amount,
			string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile
		);
		Task PaymentMediaCreateWebCardSabadellAsync(PaymentMediaCreateWebCardSabadellArguments arguments);
        Task<PaymentMediaPayResult> PaymentMediaCreateWebCardRefundSabadellAsync(string commerceCode, int publicPaymentMediaId, int publicTicketId, int publicPaymentId, decimal amount, string currency, int orderId, int terminal);
        Task<PaymentMediaPayResult> PaymentMediaCreateWebCardConfirmSabadellAsync(int publicPaymentMediaId);
        Task<PaymentMediaPayResult> PaymentMediaPayAsync(string pin, int publicPaymentMediaId, int publicTicketId, int publicPaymentId, int order, decimal amount,
			string login
		);
        Task<PaymentMediaPayResult> PaymentMediaRefundAsync(string commerceCode, string pin, int? publicPaymentMediaId, int publicTicketId, int publicPaymentId, int order, decimal amount);
        Task PaymentMediaDeleteAsync(int id);
        Task<PaymentMediaRechargeResult> RechargePaymentMediaAsync(PaymentMediaRechargeArguments arguments);
        Task<PaymentMediaGetBalanceResult> PaymentMediaGetBalanceAsync(int id);
		Task<PaymentMediaGetBalanceToRefundResult> PaymentMediaGetBalanceToRefundAsync(int id);
	}
}
