namespace PayIn.Web.App.Factories
{
	public partial class ServiceCheckPointFactory
	{
		public static string UrlApi { get { return "/Api/ServiceCheckPoint"; } }
		public static string Url { get { return "/ServiceCheckPoint"; } }

		#region Create
		public static string Create { get { return "servicecheckpointcreate"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create		

		//#region CreateCheckPoint
		//public static string CreateCheckPoint { get { return "servicecheckpointcreatecheckpoint"; } }
		//public static string CreateCheckPointApi { get { return UrlApi; } }
		//#endregion CreateCheckPoint	

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region GetAll
		public static string GetAll { get { return "servicecheckpointgetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region GetAllItem
		public static string GetItemChecks { get { return "servicecheckpointgetitemchecks"; } }
		public static string GetItemChecksApi { get { return UrlApi + "/Item"; } }
		#endregion GetAllItem

		#region GetAllCheckPoint
		public static string GetAllCheckPoint { get { return "servicecheckpointgetallcheckpoint"; } }
		public static string GetAllApiCheckPoint { get { return UrlApi; } }
		#endregion GetAllCheckPoint

		#region Update
		public static string Update { get { return "servicecheckpointupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region AddItem
		public static string AddItem { get { return "servicecheckpointadditem"; } }
		public static string AddItemApi { get { return UrlApi + "/Item"; } }
		#endregion AddItem

		#region Delete
		public static string Delete { get { return "servicecheckpointdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete

		#region Selector
		public static string RetrieveSelectorApi { get { return UrlApi + "/Selector"; } }
		#endregion Selector

		#region SelectorCheck
		public static string RetrieveSelectorCheckApi { get { return UrlApi + "/SelectorCheck"; } }
		#endregion SelectorCheck
	}
}