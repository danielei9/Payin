namespace PayIn.Web.App.Factories
{
	public partial class ControlPresenceFactory
	{
		public static string UrlApi { get { return "/Api/ControlPresence"; } }
		public static string Url { get { return "/ControlPresence"; } }

		#region Create
		public static string Create { get { return "controlpresencecreate"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region Graph
		public static string GetGraphName { get { return "controlpresencegethours"; } }
		public static string GetGraphApi { get { return UrlApi + "/Hours"; } }
		#endregion Graph

		#region GetAll
		public static string GetAll { get { return "controlpresencegetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		public static string GetAllCsv { get { return GetAllApi + "/csv"; } }
		#endregion GetAll

		#region Update
		public static string Update { get { return "controlpresenceupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region Delete
		public static string Delete { get { return "controlpresencedelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete
	}
}