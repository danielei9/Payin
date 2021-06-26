
namespace PayIn.Web.App.Factories
{
	public partial class ServiceZoneFactory
	{
		public static string UrlApi { get { return "/Api/ServiceZone"; } }
		public static string Url { get { return "/ServiceZone"; } }

		#region Selector
		public static string RetrieveSelectorApi { get { return UrlApi + "/Selector"; } }
		#endregion Selector
	}
}
