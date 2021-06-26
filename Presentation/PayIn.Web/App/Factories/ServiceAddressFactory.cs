
namespace PayIn.Web.App.Factories
{
	public partial class ServiceAddressFactory
	{
		public static string UrlApi { get { return "/Api/ServiceAddress"; } }
		public static string UrlNameApi { get { return "/Api/ServiceAddressName"; } }
		public static string Url { get { return "/ServiceAddress"; } }

		//#region Create
		//public static string CreateName { get { return "ServiceAddressCreate"; } }
		//public static string CreateApi { get { return UrlApi; } }
		//public static string Create { get { return Url + "/Create"; } }
		//public static string CreateUri { get { return "#" + Create; } }
		//public static string CreateTemplate { get { return Create; } }
		//#endregion Create

		#region Create
		public static string Create { get { return "serviceaddresscreate"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region RetrieveAll
		public static string RetrieveAllName { get { return "ServiceAddressRetrieveAll"; } }
		public static string RetrieveAllApi { get { return UrlApi; } }
		public static string RetrieveAllCsv { get { return UrlApi + "/csv"; } }
		public static string RetrieveAll { get { return Url; } }
		public static string RetrieveAllUri { get { return "#" + RetrieveAll; } }
		public static string RetrieveAllTemplate { get { return RetrieveAll; } }
		#endregion RetrieveAll

		#region GetAll
		public static string GetAll { get { return "serviceaddressgetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		public static string GetAllCsv { get { return GetAllApi + "/csv"; } }
		#endregion GetAll

		#region Delete
		public static string Delete { get { return "serviceaddressdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete

		//#region Delete
		//public static string DeleteName { get { return "Delete"; } }
		//public static string DeleteApi { get { return UrlApi; } }
		//public static string DeleteUri { get { return RetrieveAll + "#" + DeleteName; } }
		//#endregion Delete
	}
}
