namespace PayIn.Web.App.Factories
{
	public class EntranceFormValueFactory
	{
		public static string UrlApi { get { return "/Api/EntranceFormValue"; } }
		public static string Url { get { return "/EntranceFormValue"; } }

		#region GetAll
		public static string GetAllName { get { return "entranceformvaluegetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll
	}
}