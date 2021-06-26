namespace PayIn.Web.App.Factories.Bus
{
	public class StopFactory
	{
		public static string UrlApi { get { return "/Bus/Api/Stop"; } }
		public static string Url { get { return "/Bus/Stop"; } }
		
        #region ByLine
        public static string ByLineName { get { return "busstopgetbyline"; } }
        public static string ByLineApi { get { return UrlApi + "/ByLine"; } }
        #endregion ByLine

        #region Get
        public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region GetLink
		public static string GetLinkApi { get { return UrlApi + "/Link"; } }
		#endregion GetLink

		#region Update
		public static string UpdateApi { get { return UrlApi; } }
        public static string UpdateName { get { return "busstopupdate"; } }
        #endregion Update

        #region Create
        public static string CreateApi { get { return UrlApi; } }
        public static string CreateName { get { return "busstopcreate"; } }
        #endregion Create

        #region UpdateLink
        public static string UpdateLinkApi { get { return UrlApi + "/UpdateLink"; } }
		public static string UpdateLinkName { get { return "busstopupdatelink"; } }
		#endregion UpdateLink

		#region Selector
		public static string SelectorApi { get { return UrlApi + "/Selector"; } }
		#endregion Selector

		#region Visit
		public static string VisitName { get { return "busstopvisit"; } }
		public static string VisitApi { get { return UrlApi + "/Visit"; } }
		#endregion Visit
	}
}