using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayIn.Web.App.Factories
{
	public partial class LiquidationFactory
	{
		public static string UrlApi { get { return "/Api/Liquidation"; } }
		public static string Url { get { return "/Liquidation"; } }

		#region GetAll
		public static string GetAllName { get { return "liquidationgetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Create
		public static string CreateName { get { return "liquidationcreate"; } }
		public static string CreateApi { get { return UrlApi + "/Create"; } }
		#endregion CreateUrlApi + "/Create"; 

		#region GetCreate
		public static string GetCreateApi { get { return UrlApi + "/GetCreate"; } }
		#endregion GetCreate

		#region LiquidationPay
		public static string PayName { get { return "liquidationpay"; } }
		public static string PayApi { get { return UrlApi + "/Pay"; } }
		#endregion LiquidationPay

		#region LiquidationChange
		public static string LiquidationChangeName { get { return "liquidationchange"; } }
		public static string LiquidationChangeApi { get { return UrlApi + "/Change"; } }
		#endregion LiquidationChange

		#region LiquidationOpen
		public static string LiquidationOpenName { get { return "liquidationopen"; } }
		public static string LiquidationOpenApi { get { return UrlApi + "/Open"; } }
		#endregion LiquidationOpen

        #region Open
        public static string OpenName { get { return "liquidationopen"; } }
        public static string OpenApi { get { return UrlApi + "/Open"; } }
        #endregion Open

        #region Close
        public static string CloseName { get { return "liquidationclose"; } }
        public static string CloseApi { get { return UrlApi + "/Close"; } }
        #endregion Close

        #region CreateAndPay
        public static string CreateAndPayName { get { return "liquidationcreateandpay"; } }
        public static string CreateAndPayApi { get { return UrlApi + "/CreateAndPay"; } }
        #endregion CreateAndPay

        #region CreateAccountLines
        public static string CreateAccountLinesName { get { return "liquidationcreateaccountlines"; } }
        public static string CreateAccountLinesApi { get { return UrlApi + "/AccountLines"; } }
        #endregion CreateAccountLines

        #region AddAccountLines
        public static string AddAccountLinesName { get { return "liquidationaddaccountlines"; } }
        public static string AddAccountLinesApi { get { return UrlApi + "/AddAccountLines"; } }
        #endregion AddAccountLines

        #region RemoveAccountLines
        public static string RemoveAccountLinesName { get { return "liquidationremoveaccountlines"; } }
        public static string RemoveAccountLinesApi { get { return UrlApi + "/RemoveAccountLines"; } }
        #endregion RemoveAccountLines

        #region SelectorOpened
        public static string SelectorOpenedApi { get { return UrlApi + "/SelectorOpened"; } }
        #endregion SelectorOpened
    }
}