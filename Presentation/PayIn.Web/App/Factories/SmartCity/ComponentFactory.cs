namespace PayIn.Web.App.Factories.SmartCity
{
	public partial class ComponentFactory
	{
		public static string UrlApi { get { return "/SmartCity/Api/Component"; } }
		public static string Url { get { return "/SmartCity/Component"; } }

		#region GetAll
		public static string GetAllName { get { return "componentgetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll
	}
}