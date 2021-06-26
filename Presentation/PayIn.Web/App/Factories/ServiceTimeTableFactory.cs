
namespace PayIn.Web.App.Factories
{
	public partial class ServiceTimeTableFactory
	{
		public static string UrlApi { get { return "/Api/ServiceTimeTable"; } }
		public static string Url { get { return "/ServiceTimeTable"; } }

		#region Create
		public static string Create { get { return "servicetimetablecreate"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Update
		public static string Update { get { return "servicetimetableupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region Delete
		public static string Delete { get { return "servicetimetabledelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete

		#region GetAll
		public static string GetAll { get { return "servicetimetablegetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		public static string GetAllCsv { get { return GetAllApi + "/csv"; } }
		#endregion GetAll

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		//#region RetrieveAll
		//public static string RetrieveAllName { get { return "ServiceTimeTableRetrieveAll"; } }
		//public static string RetrieveAllApi { get { return UrlApi; } }
		//public static string RetrieveAllCsv { get { return UrlApi + "/csv"; } }
		//public static string RetrieveAll { get { return Url; } }
		//public static string RetrieveAllUri { get { return "#" + RetrieveAll; } }
		//public static string RetrieveAllTemplate { get { return RetrieveAll; } }
		//#endregion RetrieveAll

		//#region Create
		//public static string CreateName { get { return "ServiceTimeTableCreate"; } }
		//public static string CreateApi { get { return UrlApi; } }
		//public static string Create { get { return Url + "/Create"; } }
		//public static string CreateUri { get { return "#" + Create; } }
		//public static string CreateTemplate { get { return Create; } }
		//#endregion Create

		//#region Update
		//public static string UpdateName { get { return "ServiceTimeTableUpdate"; } }
		//public static string UpdateApi { get { return UrlApi; } }
		//public static string Update { get { return Url + "/Update"; } }
		//public static string UpdatePopup { get { return Url + "#Update"; } }
		//public static string UpdateUri { get { return "#" + Update; } }
		//public static string UpdatePopupUri { get { return UpdatePopup; } }
		//public static string UpdateTemplate { get { return Update; } }
		//#endregion Update

		//#region Delete
		//public static string DeleteName { get { return "Delete"; } }
		//public static string DeleteApi { get { return UrlApi; } }
		//public static string DeleteUri { get { return RetrieveAll + "#" + DeleteName; } }
		//#endregion Delete

		//#region Retrieve
		//public static string RetrieveApi { get { return UrlApi; } }
		//#endregion
	}
}
