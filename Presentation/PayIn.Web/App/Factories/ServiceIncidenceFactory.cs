namespace PayIn.Web.App.Factories
{
	public class ServiceIncidenceFactory
	{
		public static string Url { get { return "/ServiceIncidence"; } }
		public static string UrlApi { get { return "/Api/ServiceIncidence"; } }

		#region Get
		//public static string GetName { get { return "serviceincidenceget"; } }
		public static string GetApi { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "serviceincidencegetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region GetNotifications
		public static string GetNotificationsApi { get { return UrlApi + "/Notifications/:id"; } }
		#endregion GetNotifications

		#region Update
		public static string UpdateName { get { return "serviceincidenceupdate"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
		#endregion Update

		#region AddNotification
		public static string AddNotificationApi { get { return UrlApi + "/AddNotification/:id"; } }
		#endregion AddNotification

	}
}