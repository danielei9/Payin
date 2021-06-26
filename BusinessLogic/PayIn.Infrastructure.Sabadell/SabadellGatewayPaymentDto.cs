using System;
using System.Text;

namespace PayIn.Infrastructure.Sabadell
{
	public class PaymentDto
	{
		public string Version { get; private set; }
		public string Amount { get; private set; }
		public string Currency { get; private set; }
		public string Order { get; private set; }
		public string MerchantCode { get; private set; }
		public string CommerceUrl { get; private set; }
		public string CommerceName { get; private set; }
		public string Language { get; private set; }
		public string Terminal { get; private set; }
		public string TransactionType { get; private set; }
		public string MerchantData { get; private set; }
		public string Pan { get; private set; }
		public string ExpiryDate { get; private set; }
		public string Cvv2 { get; private set; }
		public string Signature { get; private set; }

		#region Constructor
		public PaymentDto(/*TicketPayArguments command, PaymentMedia paymentMedia, PaymentGateway paymentGateway, string commerceCode, string secretKey*/)
		{
			//if (command.Amount >= 10000000000)
			//	throw new XpException(TicketResources.AmountLimitException);

			//var year = paymentMedia.ExpirationYear.ToString();
			//var month = paymentMedia.ExpirationMonth.ToString();

			//command.Id = command.Id + 1000;

			//Version = "1.0";
			//Amount = command.Currency == "YEN" ? Math.Floor(command.Amount).ToString() : Math.Floor(command.Amount * 100).ToString();
			//Currency = SabadellGatewayFunctions.GetCurrencyToSabadell(command.Currency);
			//Order = command.Id.ToString("#0000");
			//MerchantCode = commerceCode;
			//CommerceUrl = "https://pruebaCom.jsp"; // Url de respuesta
			//CommerceName = command.SupplierName;
			//Language = SabadellGatewayFunctions.GetLanguageToSabadell();
			//Terminal = "2";
			//TransactionType = "A";
			//MerchantData = command.Title.Length > 1024 ? command.Title.Substring(0, 1024) : command.Title;
			//Pan = paymentMedia.Number; // "4548812049400004";
			//ExpiryDate = year.Substring(year.Length - 2, 2) + month; //"1212"; // AAMM
			////Cvv2 = paymentMedia.Cvv; //"123"; IT's not necessary
			//Signature = SabadellGatewayFunctions.Sha(Amount + Order + MerchantCode + Currency + Pan + Cvv2 + TransactionType + secretKey); // Pag.46 
		}
		#endregion Constructor

		#region ToXml
		public string ToXml()
		{
			var request = new StringBuilder();
			/*request.AppendFormat("<DATOSENTRADA>");
			request.AppendFormat("<DS_Version>{0}</DS_Version>", Version);
			request.AppendFormat("<DS_MERCHANT_AMOUNT>{0}</DS_MERCHANT_AMOUNT>", Amount);
			request.AppendFormat("<DS_MERCHANT_CURRENCY>{0}</DS_MERCHANT_CURRENCY>", Currency);
			request.AppendFormat("<DS_MERCHANT_ORDER>{0}</DS_MERCHANT_ORDER>", Order);
			request.AppendFormat("<DS_MERCHANT_MERCHANTCODE>{0}</DS_MERCHANT_MERCHANTCODE>", MerchantCode);
			request.AppendFormat("<DS_MERCHANT_MERCHANTURL>{0}</DS_MERCHANT_MERCHANTURL>", CommerceUrl);
			if (!CommerceName.IsNullOrEmpty())
				request.AppendFormat("<DS_MERCHANT_MERCHANTNAME>{0}</DS_MERCHANT_MERCHANTNAME>", CommerceName);
			if (!Language.IsNullOrEmpty())
				request.AppendFormat("<DS_MERCHANT_CONSUMERLANGUAGE>{0}</DS_MERCHANT_CONSUMERLANGUAGE>", Language);
			request.AppendFormat("<DS_MERCHANT_MERCHANTSIGNATURE>{0}</DS_MERCHANT_MERCHANTSIGNATURE>", Signature);
			request.AppendFormat("<DS_MERCHANT_TERMINAL>{0}</DS_MERCHANT_TERMINAL>", Terminal);
			request.AppendFormat("<DS_MERCHANT_TRANSACTIONTYPE>{0}</DS_MERCHANT_TRANSACTIONTYPE>", TransactionType);
			if (!MerchantData.IsNullOrEmpty())
				request.AppendFormat("<DS_MERCHANT_MERCHANTDATA>{0}</DS_MERCHANT_MERCHANTDATA>", MerchantData);
			if (!Pan.IsNullOrEmpty())
				request.AppendFormat("<DS_MERCHANT_PAN>{0}</DS_MERCHANT_PAN>", Pan);
			if (!ExpiryDate.IsNullOrEmpty())
				request.AppendFormat("<DS_MERCHANT_EXPIRYDATE>{0}</DS_MERCHANT_EXPIRYDATE>", ExpiryDate);
			if (!Cvv2.IsNullOrEmpty())
				request.AppendFormat("<DS_MERCHANT_CVV2>{0}</DS_MERCHANT_CVV2>", Cvv2);
			request.AppendFormat("</DATOSENTRADA>");*/

			return request.ToString();
		}
		#endregion ToXml
	}
}