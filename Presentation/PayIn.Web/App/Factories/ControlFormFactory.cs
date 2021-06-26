
namespace PayIn.Web.App.Factories
{
	public class ControlFormFactory
	{
		public static string UrlApi { get { return "/Api/ControlForm"; } }
		public static string Url { get { return "/ControlForm"; } }

		#region Create
		public static string CreateName { get { return "controlformcreate"; } }
		public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region GetSelector
		public static string GetSelectorApi { get { return UrlApi + "/Selector"; } }
		#endregion GetSelector

		#region GetAll
		public static string GetAllName { get { return "controlformgetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		public static string GetAllCsv { get { return GetAllApi + "/csv"; } }
		#endregion GetAll

		#region Update
		public static string UpdateName { get { return "controlformupdate"; } }
		public static string Update { get { return Url + "/Update"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region Delete
		public static string DeleteName { get { return "controlformdelete"; } }
		public static string Delete { get { return Url + "/Delete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete
	}
}