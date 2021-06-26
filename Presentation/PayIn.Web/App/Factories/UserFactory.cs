namespace PayIn.Web.App.Factories
{
	public class UserFactory
	{
		public static string UrlApi { get { return "/Api/User"; } }
		public static string Url { get { return "/User"; } }

		#region CreateNotification
		public static string CreateNotificationName { get { return "usercreatenotification"; } }
		public static string CreateNotification { get { return Url + "/CreateNotification"; } }
		public static string CreateNotificationApi { get { return UrlApi; } }
		#endregion CreateNotification	

		#region GetAll
		public static string GetAllName { get { return "usergetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll
	}
}
