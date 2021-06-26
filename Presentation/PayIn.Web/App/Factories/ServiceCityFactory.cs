
namespace PayIn.Web.App.Factories
{
	public partial class ServiceCityFactory
	{
		public static string UrlApi { get { return "/Api/City"; } }
		public static string Url { get { return "/City"; } }

		#region Selector
		public static string RetrieveSelectorApi { get { return UrlApi + "/Selector"; } }
		#endregion Selector

	}
}
