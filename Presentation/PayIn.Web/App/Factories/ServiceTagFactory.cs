namespace PayIn.Web.App.Factories
{
	public partial class ServiceTagFactory
	{
		public static string UrlApi { get { return "/Api/ServiceTag"; } }
		public static string Url { get { return "/ServiceTag"; } }

		#region Create
		public static string Create { get { return "servicetagcreate"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region GetAll
		public static string GetAll { get { return "servicetaggetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Update
		public static string Update { get { return "servicetagupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region Delete
		public static string Delete { get { return "servicetagdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete

		#region Selector
		public static string RetrieveSelectorApi { get { return UrlApi + "/Selector"; } }
		#endregion Selector
	}
}