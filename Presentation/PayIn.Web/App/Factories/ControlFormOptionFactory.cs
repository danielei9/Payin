using System;

namespace PayIn.Web.App.Factories
{
	public class ControlFormOptionFactory
	{
		public static string UrlApi { get { return "/Api/ControlFormOption"; } }
		public static string Url { get { return "/ControlFormOption"; } }

		#region GetAll
		public static string GetAllName { get { return "controlformoptiongetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Get
		public static string GetApi { get { return UrlApi; } }
		public static string GetApiTemplate { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region Create
		public static string CreateName { get { return "controlformoptioncreate"; } }
		public static string Create (string argumentId = "") { return CreateName + (argumentId.IsNullOrEmpty() ? "" : "({id:\"" + argumentId + "\"})"); }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Update
		public static string UpdateName { get { return "controlformoptionupdate"; } }
		public static string Update { get { return Url + "/Update"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
		#endregion Update

		#region Delete
		public static string Delete { get { return "controlformoptiondelete"; } }
		public static string DeleteApi { get { return UrlApi + "/:id"; } }
		#endregion Delete
	}
}