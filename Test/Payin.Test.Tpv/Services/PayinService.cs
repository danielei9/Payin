using Microsoft.AspNet.SignalR.Client;
using Payin.Test.Tpv;
using System.Threading.Tasks;
using System.Windows;
using Xp.Infrastructure.Http;
#if !(DEBUG || RELEASE)
using Microsoft.WindowsAzure.ServiceRuntime;
#endif

namespace PayIn.TestTpv
{
	public class PayinService : BaseService
	{
#if DEBUG || RELEASE
		public override string BaseAddress { get { return "http://localhost:8080/"; } }
#else
		public override string BaseAddress { get { return "http://" + RoleEnvironment.Roles["Payin.Web.Internal"].Instances[RoleEnvironment.Roles["Payin.Web"].Instances.IndexOf(RoleEnvironment.CurrentRoleInstance)].InstanceEndpoints["Http"].IPEndpoint.ToString() + "/"; } }
#endif

#region Constructors
		public PayinService(SessionData sessionData)
		{
			SessionData = sessionData;
        }
#endregion Constructors

#region LoginAsync
		public async Task<string> LoginAsync()
		{
			var result = await Server.LoginTpvAsync("tpv@pay-in.es", "Pa$$w0rd");
			((SessionData) SessionData).Token = result;

			return result;
		}
#endregion LoginAsync

#region SignalR
		public async Task ConnectSignalR()
		{
			await Server.ConnectSignalR<PushMessage>("SendMessage", x => {
				MessageBox.Show(x.Message);
			});
		}
#endregion SignalR

		//#region UserHasPayment
		//public async Task<bool> UserHasPaymentAsync()
		//{
		//	var result = await Server.GetAsync("Internal/User/HasPayment", new UserHasPaymentArguments());
		//	return Convert.ToBoolean(result["hasPayment"]);
		//}
		//#endregion UserHasPayment

		//#region UserUpdatePinAsync
		//public async Task UserUpdatePinAsync(string oldPin, string pin, string confirmPin)
		//{
		//	var result = await Server.PostAsync("Internal/User/Pin", new UserUpdatePinArguments
		//	(
		//		oldPin: oldPin,
		//		pin: pin,
		//		confirmPin: confirmPin
		//	));
		//}
		//#endregion UserUpdatePinAsync

		//#region UserCheckPinAsync
		//public async Task<bool> UserCheckPinAsync(string pin) 
		//{
		//	var result = await Server.GetAsync("Internal/User/CheckPin", new UserCheckPinArguments
		//	(
		//		pin: pin
		//	));

		//	return Convert.ToBoolean(result["checkPin"]);
		//}
		//#endregion UserCheckPinAsync

		//#region PaymentMediaCreateWebCardAsync
		//public async Task<dynamic> PaymentMediaCreateWebCardAsync(string pin, string name, int orderId, int publicPaymentMediaId, int publicTicketId, int publicPaymentId, string bankEntity, 
		//	string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile)
		//{
		//	return await Server.PostAsync("Internal/PaymentMedia/WebCard", new PaymentMediaCreateWebCardArguments(
		//		pin: pin,
		//		name: name,
		//		orderId: orderId,
		//		publicPaymentMediaId: publicPaymentMediaId,
		//		publicTicketId: publicTicketId,
		//		publicPaymentId: publicPaymentId,
		//		bankEntity: bankEntity,
		//		deviceManufacturer: deviceManufacturer,
		//		deviceModel: deviceModel,
		//		deviceName: deviceName,
		//		deviceSerial: deviceSerial,
		//		deviceId: deviceId,
		//		deviceOperator: deviceOperator,
		//		deviceImei: deviceImei,
		//		deviceMac: deviceMac,
		//		operatorSim: operatorSim,
		//		operatorMobile: operatorMobile
		//	));
		//}
		//#endregion PaymentMediaCreateWebCardAsync

		//#region PaymentMediaCreateWebCardSabadellAsync
		//public async Task PaymentMediaCreateWebCardSabadellAsync(PaymentMediaCreateWebCardSabadellArguments arguments)
		//{
		//	await Server.PostAsync("Sabadell/WebCard", arguments);
		//}
		//#endregion PaymentMediaCreateWebCardSabadellAsync

		//#region PaymentMediaCreateWebCardRefundSabadellAsync
		//public async Task<PaymentMediaPayResult> PaymentMediaCreateWebCardRefundSabadellAsync(int publicPaymentMediaId, int publicTicketId, int publicPaymentId, decimal amount, string currency, int orderId, int terminal)
		//{
		//	var result = await Server.PostAsync<PaymentMediaPayResult>("Sabadell/WebCardRefund", new PaymentMediaCreateWebCardRefundSabadellArguments(
		//		publicPaymentMediaId: publicPaymentMediaId,
		//		publicTicketId: publicTicketId,
		//		publicPaymentId: publicPaymentId,
		//		amount: amount,
		//		currency: currency,
		//		orderId: orderId,
		//		terminal: terminal
		//	));
		//	return result;
		//}
		//#endregion PaymentMediaCreateWebCardRefundSabadellAsync

		//#region PaymentMediaPayAsync
		//public async Task<PaymentMediaPayResult> PaymentMediaPayAsync(string pin, int publicPaymentMediaId, int publicTicketId, int publicPaymentId, int order, decimal amount)
		//{
		//	var result = await Server.PostAsync<PaymentMediaPayResult>("Internal/PaymentMedia/Pay", new PaymentMediaPayArguments(
		//		pin: pin,
		//		publicPaymentMediaId: publicPaymentMediaId,
		//		publicTicketId: publicTicketId,
		//		publicPaymentId: publicPaymentId,
		//		order: order,
		//		amount: amount
		//	));
		//	return result;
		//}
		//#endregion PaymentMediaPayAsync

		//#region PaymentMediaRefundAsync
		//public async Task<PaymentMediaPayResult> PaymentMediaRefundAsync(string pin, int publicPaymentMediaId, int publicTicketId, int publicPaymentId, int order, decimal amount)
		//{
		//	var result = await Server.PostAsync<PaymentMediaPayResult>("Internal/PaymentMedia/Refund", new PaymentMediaRefundArguments(
		//		pin: pin,
		//		publicPaymentMediaId: publicPaymentMediaId,
		//		publicTicketId: publicTicketId,
		//		publicPaymentId: publicPaymentId,
		//		order: order,
		//		amount: amount
		//	));
		//	return result;
		//}
		//#endregion PaymentMediaRefundAsync

		//#region PaymentMediaDeleteAsync
		//public async Task PaymentMediaDeleteAsync(int id)
		//{
		//	await Server.DeleteAsync("Internal/PaymentMedia", id);
		//}
		//#endregion PaymentMediaDeleteAsync
	}
}
