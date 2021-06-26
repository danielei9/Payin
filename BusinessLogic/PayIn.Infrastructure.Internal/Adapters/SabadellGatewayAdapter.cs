using PayIn.Common;
using PayIn.Domain.Internal;
using PayIn.Domain.Internal.Infrastructure;
using PayIn.Infrastructure.Sabadell;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xp.Domain;

namespace PayIn.Infrastructure.Internal.Adapters
{
	public class SabadellGatewayAdapter : IPaymentGatewayAdapter
	{
		// Payin
		public const string CommerceNamePayIn = "Payment Innovation Network S.L.";
		public const string CommerceCodePayIn = "327624508";
		// FACCA
		public const string CommerceNameFACCA = "Falla Almirante Cadarso";
		public const string CommerceCodeFACCA = "344657044";

#if TEST || DEBUG || EMULATOR
		public const string CommerceSecretKey256PayIn = "sq7HjrUOBfKmC576ILgskD5srU870gJ7";
		public const string CommerceSecretKey256FACCA = "sq7HjrUOBfKmC576ILgskD5srU870gJ7";
		public const string Uri = "https://sis-t.redsys.es:25443/sis/operaciones";
		public const string CommerceUrl = "http://payin-test.cloudapp.net";
		public const string CommerceUrlOkPayIn = "http://payin-test.cloudapp.net";
		public const string CommerceUrlOkFACCA = "http://payin-test.cloudapp.net/Shop#/Shop/ByConcession/97";
#elif HOMO
		public const string CommerceSecretKey256PayIn = "sq7HjrUOBfKmC576ILgskD5srU870gJ7";
		public const string CommerceSecretKey256FACCA = "sq7HjrUOBfKmC576ILgskD5srU870gJ7";
		public const string Uri = "https://sis-t.redsys.es:25443/sis/operaciones";
		public const string CommerceUrl = "http://payin-homo.cloudapp.net";
		public const string CommerceUrlOkPayIn = "http://payin-test.cloudapp.net";
		public const string CommerceUrlOkFACCA = "";
#elif FALLAS
		public const string CommerceSecretKey256PayIn = ""; //Cambiar también en SabadellGatewayFunctions
		public const string CommerceSecretKey256FACCA = ""; //Cambiar también en SabadellGatewayFunctions
		public const string Uri = "https://sis.REDSYS.es/sis/operaciones";
		public const string CommerceUrl = "http://fallas.pay-in.es";
		public const string CommerceUrlOkPayIn = "";
		public const string CommerceUrlOkFACCA = "http://facca.pay-in.es";
#else
		public const string CommerceSecretKey256PayIn = ""; //Cambiar también en SabadellGatewayFunctions
		public const string CommerceSecretKey256FACCA = "";
		public const string Uri = "https://sis.REDSYS.es/sis/operaciones";
		public const string CommerceUrl = "https://control.pay-in.es";
		public const string CommerceUrlOkPayIn = "";
		public const string CommerceUrlOkFACCA = "http://facca.pay-in.es";
#endif

		private readonly IEntityRepository<PaymentMedia> PaymentMediaRepository;
		private readonly IEntityRepository<PaymentGateway> PaymentGatewayRepository;

		#region Constructors
		public SabadellGatewayAdapter(IEntityRepository<PaymentMedia> paymentMediaRepository, IEntityRepository<PaymentGateway> paymentGatewayRepository)
		{
			if (paymentMediaRepository == null)
				throw new ArgumentNullException("paymentMediaRepository");
			PaymentMediaRepository = paymentMediaRepository;

			if (paymentGatewayRepository == null)
				throw new ArgumentNullException("paymentGatewayRepository");
			PaymentGatewayRepository = paymentGatewayRepository;
		}
		#endregion Constructors

		#region WebCardRequestAsync
		public async Task<string> WebCardRequestAsync(string commerceCode, int? paymentMediaId, int? publicPaymentMediaId, int publicTicketId, int publicPaymentId, int orderId, PaymentMediaCreateType paymentMediaCreateType, decimal amount)
		{
			return await Task.Run(() => SabadellGatewayFunctions.CreateWebPaymentRequest(
				paymentMediaId: paymentMediaId,
				publicPaymentMediaId: publicPaymentMediaId,
				publicTicketId: publicTicketId,
				publicPaymentId: publicPaymentId,
				orderId: orderId,
				paymentMediaCreateType: paymentMediaCreateType,
				transactionType: "0",
				commerceUrl: CommerceUrl + "/Sabadell/WebCard",
				commerceCode:
					commerceCode == CommerceCodeFACCA ? CommerceCodeFACCA :
					CommerceCodePayIn,
				commerceSecretKey:
					commerceCode == CommerceCodeFACCA ? CommerceSecretKey256FACCA :
					CommerceSecretKey256PayIn,
				commerceName:
					commerceCode == CommerceCodeFACCA ? CommerceNameFACCA :
					CommerceNamePayIn,
				urlOk:
					commerceCode == CommerceCodeFACCA ?
						CommerceUrlOkFACCA :
						CommerceUrlOkPayIn,
				amount: amount
			));
		}
		#endregion WebCardRequestAsync

		#region PayAsync
		public async Task<string> PayAsync(int paymentMediaId, int publicPaymentMediaId, int publicTicketId, int publicPaymentId, string cardIdentifier, int orderId, decimal amount)
		{
			var commerceCode = CommerceCodePayIn;

			var xml = SabadellGatewayFunctions.CreatePaymentRequest(
				cardIdentifier: cardIdentifier,
				paymentMediaId: paymentMediaId,
				publicPaymentMediaId: publicPaymentMediaId,
				publicTicketId: publicTicketId,
				publicPaymentId: publicPaymentId,
				orderId: orderId,
				paymentMediaCreateType: PaymentMediaCreateType.DirectPay,
				transactionType: "0",
				commerceCode:
					commerceCode == CommerceCodeFACCA ? CommerceCodeFACCA :
					CommerceCodePayIn,
				commerceSecretKey:
					commerceCode == CommerceCodeFACCA ? CommerceSecretKey256FACCA :
					CommerceSecretKey256PayIn,
				commerceName:
					commerceCode == CommerceCodeFACCA ? CommerceNameFACCA :
					CommerceNamePayIn,
				amount: amount
			);

			var result = await SendAsync("entrada=" + xml);
			return result;
		}
		#endregion PayAsync

		#region RefundAsync
		public async Task<string> RefundAsync(string commerceCode, int? paymentMediaId, int? publicPaymentMediaId, int publicTicketId, int publicPaymentId, string cardIdentifier, int orderId, decimal amount)
		{
			var xml = SabadellGatewayFunctions.CreatePaymentRequest(
				cardIdentifier: cardIdentifier,
				paymentMediaId: paymentMediaId,
				publicPaymentMediaId: publicPaymentMediaId,
				publicTicketId: publicTicketId,
				publicPaymentId: publicPaymentId,
				orderId: orderId,
				paymentMediaCreateType: PaymentMediaCreateType.DirectPay,
				transactionType: "3",
				commerceCode:
					commerceCode == CommerceCodeFACCA ? CommerceCodeFACCA :
					CommerceCodePayIn,
				commerceSecretKey:
					commerceCode == CommerceCodeFACCA ? CommerceSecretKey256FACCA :
					CommerceSecretKey256PayIn,
				commerceName:
					commerceCode == CommerceCodeFACCA ? CommerceNameFACCA :
					CommerceNamePayIn,
				amount: amount
			);

			var result = await SendAsync("entrada=" + xml);
			return result;
		}
		#endregion RefundAsync

		#region VerifyCommerceCode
		public bool VerifyCommerceCode(string commerceCodeToVerify)
		{
			return
				(CommerceCodePayIn == commerceCodeToVerify) ||
				(CommerceCodeFACCA == commerceCodeToVerify);
		}
		#endregion VerifyCommerceCode

		#region VerifyResponse
		public bool VerifyResponse(decimal amount, int orderId, string commerceCode, string currency, string response, string cardNumberHash, string transactionType, bool securePayment, string signature)
		{
			var orderString = orderId.ToString("#0000");
			var securePaymentString = securePayment ? "1" : "0";
			var commerceSecretKey =
					commerceCode == CommerceCodeFACCA ? CommerceSecretKey256FACCA :
					CommerceSecretKey256PayIn;

			return (SabadellGatewayFunctions.Sha(amount + orderString + commerceCode + currency + response + cardNumberHash + transactionType + securePaymentString + commerceSecretKey) == signature);
		}
		public bool VerifyResponse(string data, string signature)
		{
			var commerceCode = CommerceCodePayIn;
			var commerceSecretKey =
					commerceCode == CommerceCodeFACCA ? CommerceSecretKey256FACCA :
					CommerceSecretKey256PayIn;

			// Digest=SHA-1(Ds_ Amount + Ds_ Order + Ds_MerchantCode + Ds_ Currency + Ds _Response + CLAVE SECRETA)
			return (SabadellGatewayFunctions.Sha(data + commerceSecretKey) == signature);
		}
		#endregion VerifyResponse

		#region SendAsync
		private async Task<string> SendAsync(string request)
		{
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

			using (var client = new HttpClient())
			{
				var content = new StringContent(request, Encoding.UTF8);
				content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded"); // Content-type: application/x-www-form-urlencoded

				var response = (await client.PostAsync(new Uri(Uri), content))
					.ThrowException();

				var result = (await response.Content.ReadAsStringAsync());
				return result;
			}
		}
		#endregion SendAsync
	}
}
