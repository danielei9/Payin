namespace PayIn.Web.App.Factories.Bus
{
	public class RequestFactory
	{
		public static string UrlApi { get { return "/Bus/Api/Request"; } }
		public static string Url { get { return "/Bus/Request"; } }
		
		#region Create
		public static string CreateName { get { return "busrequestcreate"; } }
		public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Delete
		public static string DeleteName { get { return "busrequestdelete"; } }
		public static string Delete { get { return Url + "/Delete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete
	}
}