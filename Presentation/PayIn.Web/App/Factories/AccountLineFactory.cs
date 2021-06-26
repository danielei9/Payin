namespace PayIn.Web.App.Factories
{
	public partial class AccountLineFactory
	{
		public static string UrlApi { get { return "/Api/AccountLine"; } }
		public static string Url { get { return "/AccountLine"; } }

		#region GetAll
		public static string GetByLiquidationName { get { return "accountlinegetbyliquidation"; } }
        public static string GetByLiquidationApi { get { return UrlApi + "/Liquidation"; } }
        public static string GetByLiquidation(string liquidationId) { return "accountlinegetbyliquidation({id:" + liquidationId + "})"; }
        #endregion GetAll

        #region GetByLogBook
        public static string GetByLogBookName { get { return "accountlinegetbylogbook"; } }
        public static string GetByLogBookApi { get { return UrlApi + "/LogBook"; } }
        #endregion GetByLogBook

        #region GetByAccounts
        public static string GetByAccountsName { get { return "accountlinegetbyaccounts"; } }
        public static string GetByAccountsApi { get { return UrlApi + "/Accounts"; } }
        #endregion GetByAccounts

        #region GetByCash
        public static string GetByCashName { get { return "accountlinegetbycash"; } }
        public static string GetByCashApi { get { return UrlApi + "/Cash"; } }
        #endregion GetByServiceCards

        #region GetByServiceCards
        public static string GetByServiceCardsName { get { return "accountlinegetbyservicecards"; } }
        public static string GetByServiceCardsApi { get { return UrlApi + "/ServiceCards"; } }
        #endregion GetByServiceCards

        #region GetByCreditCards
        public static string GetByCreditCardsName { get { return "accountlinegetbycreditcards"; } }
        public static string GetByCreditCardsApi { get { return UrlApi + "/CreditCards"; } }
        #endregion GetByCreditCards

        #region GetByProducts
        public static string GetByProductsName { get { return "accountlinegetbyproducts"; } }
        public static string GetByProductsApi { get { return UrlApi + "/Products"; } }
        #endregion GetByProducts

        #region GetByEntranceTypes
        public static string GetByEntranceTypesName { get { return "accountlinegetbyentrancetypes"; } }
        public static string GetByEntranceTypesApi { get { return UrlApi + "/EntranceTypes"; } }
        #endregion GetByEntranceTypes

        #region GetByOthers
        public static string GetByOthersName { get { return "accountlinegetbyothers"; } }
        public static string GetByOthersApi { get { return UrlApi + "/Others"; } }
        #endregion GetByOthers
    }
}