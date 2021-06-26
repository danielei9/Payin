namespace PayIn.Web.App.Factories
{
	public partial class TransportTitleFactory
	{
		public static string UrlApi { get { return "/Api/TransportTitle"; } }		
		public static string Url { get { return "/TransportTitle"; } }		

		#region GetAll
		public static string GetAllName { get { return "transporttitlegetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region Create
		public static string CreateName { get { return "transporttitlecreate"; } }
		public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Update
		public static string UpdateName { get { return "transporttitleupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region UpdatePrice
		public static string UpdatePriceName { get { return "transporttitlepriceupdate"; } }
		public static string UpdatePriceApi { get { return UrlApi + "/PriceUpdate"; } }
		#endregion UpdatePrice	

		#region Delete
		public static string Delete { get { return "transporttitledelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion

		#region Selector
		public static string RetrieveSelectorApi { get { return UrlApi + "/Selector"; } }
		public static string RetrieveSelectorTransporConcessionApi { get { return UrlApi + "/Selector/TConcessions"; } }
		#endregion Selector

		#region Selector
		public static string RetrieveSelectorTitleApi { get { return UrlApi + "/SelectorTitle"; } }
		#endregion Selector

	}
}
