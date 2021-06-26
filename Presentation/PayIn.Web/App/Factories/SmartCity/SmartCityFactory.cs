namespace PayIn.Web.App.Factories.SmartCity
{
	public partial class SmartCityFactory
	{
		public static string UrlApi { get { return "/SmartCity/Api/SmartCity"; } }
		public static string Url { get { return "/SmartCity"; } }

		#region GetAll
		public static string GetAllName { get { return "smartcitygetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll
	}
}