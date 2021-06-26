using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayIn.Web.App.Factories
{
	public partial class PaymentFactory
	{
		public static string UrlApi { get { return "/Api/Payment"; } }
		public static string Url { get { return "/Payment"; } }

		#region GetAll
		public static string GetAllName { get { return "paymentgetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region GetCharges
		public static string GetChargesName { get { return "paymentgetcharges"; } }
		public static string GetChargesApi { get { return UrlApi + "/Charges"; } }
		#endregion GetCharges

		#region Refund
		public static string RefundName { get { return "paymentrefundpopup"; } }
		public static string RefundApi { get { return UrlApi + "/v1/Refund"; } }

		#endregion Refund

		#region PaymentDetails
		public static string PaymentDetailsName { get { return "paymentpaymentdetails"; } }
		public static string PaymentDetailsApi { get { return UrlApi; } }
		#endregion PaymentDetails

		#region PaymentGraph
		public static string PaymentGraphName { get { return "paymentgraph"; } }
		public static string PaymentGraph { get { return Url; } }
		public static string PaymentGraphApi { get { return UrlApi + "/Graph"; } }
		#endregion PaymentGraph

		#region LiquidationPayments
		public static string LiquidationPaymentsName { get { return "liquidationpayments"; } }
		public static string LiquidationPaymentsState(string liquidationId, string concessionId) { return LiquidationPaymentsName + "({liquidationId:" + liquidationId + ",concessionId:" + concessionId + "})"; }
		public static string LiquidationPaymentsApi { get { return UrlApi + "/LiquidationPayments"; } }
		#endregion LiquidationPayments

		#region UnliquidatedPayments
		public static string UnliquidatedName { get { return "paymentunliquidated"; } }
		public static string UnliquidatedApi { get { return UrlApi + "/Unliquidated"; } }
		#endregion UnliquidatedPayments

		#region CardPayments
		public static string CardPaymentsName { get { return "paymentgetpaymentsbycard"; } }
		public static string CardPaymentsApi { get { return UrlApi + "/CardPayments"; } }
		#endregion CardPayments
	}
}