namespace PayIn.Web.App.Factories
{
	public class ActivityFactory
	{
		public static string UrlApi { get { return "/Api/Activity"; } }
		public static string Url { get { return "/Activity"; } }

		#region Get
		public static string Get { get { return UrlApi; } }
		public static string GetApi { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "activitygetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Create
		public static string CreateName { get { return "activitycreate"; } }
		public static string Create(string eventId = "") { return CreateName + ("({id:\"" + eventId + "\"})"); }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Delete
		public static string Delete { get { return "activitydelete"; } }
		public static string DeleteApi { get { return UrlApi + "/:id"; } }
		#endregion Delete

		#region Update
		public static string UpdateName { get { return "activityupdate"; } }
		public static string Update { get { return Url + "/Update"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
		#endregion Update
	}
}