namespace PayIn.Web.App.Factories.SmartCity
{
	public partial class DeviceFactory
	{
		public static string UrlApi { get { return "/SmartCity/Api/Device"; } }
		public static string Url { get { return "/SmartCity/Device"; } }

		#region GetAll
		public static string GetAllName { get { return "devicegetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll
	}
}