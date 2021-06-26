namespace PayIn.Web.App.Factories.Bus
{
	public class GraphFactory
	{
		public static string UrlApi { get { return "/Bus/Api/Graph"; } }
		public static string Url { get { return "/Bus/Graph"; } }
		
		#region GetAll
		public static string GetAllName { get { return "busgraphgetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll
	}
}