using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ApiPaymentMediaPaySabadellArguments : IArgumentsBase
	{
		public string Ds_Date { get; set; } // Formato: DD-MM-AAAA
		public string Ds_Hour { get; set; } // Formato HH-MM
		public int    Ds_Amount { get; set; }
		public int    Ds_Currency { get; set; }
		public int    Ds_Order { get; set; }
		public int    Ds_MerchantCode { get; set; }
		public int    Ds_Terminal { get; set; }
		public string Ds_Signature { get; set; }
		public int    Ds_Response { get; set; }
		public string Ds_TransactionType { get; set; }
		public bool   Ds_SecurePayment { get; set; }
		public string Ds_MerchantData { get; set; }
		public int    Ds_Card_Country { get; set; }
		public string Ds_AuthorisationCode { get; set; }
		public int    Ds_ConsumerLanguage { get; set; }
		public string Ds_Card_Type { get; set; }

		#region Constructors
		public ApiPaymentMediaPaySabadellArguments(string ds_Date, string ds_Hour, int ds_Amount, int ds_Currency, int ds_Order, int ds_MerchantCode, int ds_Terminal, string ds_Signature, int ds_Response, string ds_TransactionType, bool ds_SecurePayment, string ds_MerchantData, int ds_Card_Country, string ds_AuthorisationCode, int ds_ConsumerLanguage, string ds_Card_Type)
		{
			Ds_Date = ds_Date;
			Ds_Hour = ds_Hour;
			Ds_Amount = ds_Amount;
			Ds_Currency = ds_Currency;
			Ds_Order = ds_Order;
			Ds_MerchantCode = ds_MerchantCode;
			Ds_Terminal = ds_Terminal;
			Ds_Signature = ds_Signature;
			Ds_Response = ds_Response;
			Ds_TransactionType = ds_TransactionType;
			Ds_SecurePayment = ds_SecurePayment;
			Ds_MerchantData = ds_MerchantData;
			Ds_Card_Country = ds_Card_Country;
			Ds_AuthorisationCode = ds_AuthorisationCode;
			Ds_ConsumerLanguage = ds_ConsumerLanguage;
			Ds_Card_Type = ds_Card_Type;
		}
		#endregion Constructors
	}
}
