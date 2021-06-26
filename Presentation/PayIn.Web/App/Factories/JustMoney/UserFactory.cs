namespace PayIn.Web.App.Factories.JustMoney
{
	public class UserFactory
	{
		public static string UrlApi { get { return "/JustMoney/Api/User"; } }
		public static string Url { get { return "/JustMoney/User"; } }
		
		#region GetAll
		public static string GetAllName { get { return "usergetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Update
		public static string UpdateName { get { return "userupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region GetDetail
		public static string GetDetailsName { get { return "usergetdetails"; } }
		public static string GetDetailsApi { get { return UrlApi + "/Details"; } }
		#endregion GetDetail
	}
}


/*
        public static string GetByLiquidationApi { get { return UrlApi + "/Liquidation"; } }
        public static string GetByLiquidation(string liquidationId) { return "accountlinegetbyliquidation({id:" + liquidationId + "})"; }

 */
