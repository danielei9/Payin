
namespace PayIn.Web.App.Factories
{
	public partial class PaymentMediaFactory
	{
		public static string UrlApi { get { return "/Api/PaymentMedia"; } }

		#region RetrieveSelector
		public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
		#endregion RetrieveSelector
	}
}
