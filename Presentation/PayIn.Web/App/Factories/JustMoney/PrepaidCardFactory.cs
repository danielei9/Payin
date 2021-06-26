namespace PayIn.Web.App.Factories.JustMoney
{
	public class PrepaidCardFactory
	{
		public static string UrlApi { get { return "/JustMoney/Api/PrepaidCard"; } }
		public static string Url { get { return "/JustMoney/PrepaidCard"; } }

		#region Activate
		public static string ActivateName { get { return "prepaidcardactivate"; } }
		public static string Activate { get { return Url + "/Activate"; } }
		public static string ActivateApi { get { return UrlApi + "/Activate"; } }
		#endregion Activate

		#region Create
		public static string CreateName { get { return "prepaidcardcreate"; } }
		public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region AddCard
		public static string AddCardName { get { return "prepaidcardaddcard"; } }
		public static string AddCard { get { return Url + "/AddCard"; } }
		public static string AddCardApi { get { return UrlApi; } }
		#endregion AddCard

		#region GetAll
		public static string GetAllName { get { return "prepaidcardgetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll
	}
}