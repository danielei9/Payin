namespace PayIn.Web.App.Factories
{
	public class ServiceNotificationFactory
	{
		public static string UrlApi { get { return "/Api/ServiceNotification"; } }
		public static string Url { get { return "/ServiceNotification"; } }

		#region GetAll
		public static string GetAllName { get { return "servicenotificationgetall"; } }
		public static string GetAll { get { return Url + "/ServiceNotification"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll		

		#region Create
		public static string CreateApi { get { return UrlApi + "/Create"; } }
		#endregion Create
	}
}