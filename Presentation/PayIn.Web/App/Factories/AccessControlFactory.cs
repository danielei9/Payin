namespace PayIn.Web.App.Factories
{
	public class AccessControlFactory
	{
		public static string UrlApi { get { return "/Api/AccessControl"; } }
		public static string Url { get { return "/AccessControl"; } }

		#region Get

		public static string Get { get { return UrlApi; } }
		public static string GetApi { get { return UrlApi + "/:id"; } }

		#endregion

		#region GetAll

		public static string GetAllName { get { return "accesscontrolgetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }

		#endregion

		#region Create

		public static string CreateName { get { return "accesscontrolcreate"; } }
		public static string Create(string eventId = "") { return CreateName + ("({id:\"" + eventId + "\"})"); }
		public static string CreateApi { get { return UrlApi; } }

		#endregion

		#region Delete

		public static string Delete { get { return "accesscontroldelete"; } }
		public static string DeleteApi { get { return UrlApi + "/:id"; } }

		#endregion

		#region Update

		public static string UpdateName { get { return "accesscontrolupdate"; } }
		public static string Update { get { return Url + "/Update"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }

		#endregion

		#region Places

		public static string PlacesName { get { return "accesscontrolplaces"; } }
		public static string Places { get { return Url + "/Places"; } }
		public static string PlacesApi { get { return UrlApi + "/Places"; } }

		#endregion

		#region Place

		public static string PlaceApi { get { return UrlApi + "/Place/:id"; } }

		#endregion

		#region Entry

		public static string EntryName { get { return "accesscontrolentry"; } }
		public static string Entry { get { return Url + "/Entry"; } }
		public static string EntryApi { get { return UrlApi + "/Entry"; } }

		#endregion

		#region EntryReset

		public static string EntryReset { get { return "accesscontrolentryreset"; } }
		public static string EntryResetApi { get { return UrlApi + "/Entry/Reset"; } }

		#endregion

		#region Weather

		public static string Weather { get { return "accesscontrolweather"; } }
		public static string WeatherApi { get { return UrlApi + "/Place/Weather"; } }

		#endregion

		#region Info

		public static string Info { get { return "accesscontrolinfo"; } }

		#endregion
	}
}