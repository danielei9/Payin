namespace PayIn.Web.App.Factories
{
	public class AccessControlEntranceFactory
	{
		public static string UrlApi { get { return "/Api/AccessControl/Entrance"; } }
		public static string Url { get { return "/AccessControl/Entrance"; } }

		#region Get

		public static string Get { get { return UrlApi; } }
		public static string GetApi { get { return UrlApi + "/:id"; } }

		#endregion

		#region GetAll

		public static string GetAllName { get { return "accesscontrolentrancegetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }

		#endregion

		#region Create

		public static string CreateName { get { return "accesscontrolentrancecreate"; } }
		public static string Create(string eventId = "") { return CreateName + ("({id:\"" + eventId + "\"})"); }
		public static string CreateApi { get { return UrlApi; } }

		#endregion

		#region Delete

		public static string Delete { get { return "accesscontrolentrancedelete"; } }
		public static string DeleteApi { get { return UrlApi + "/:id"; } }

		#endregion

		#region Update

		public static string UpdateName { get { return "accesscontrolentranceupdate"; } }
		public static string Update { get { return Url + "/Update"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }

		#endregion
	}
}