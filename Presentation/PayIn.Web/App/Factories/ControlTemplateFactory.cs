namespace PayIn.Web.App.Factories
{
	public partial class ControlTemplateFactory
	{
		public static string UrlApi { get { return "/Api/ControlTemplate"; } }
		public static string Url { get { return "/ControlTemplate"; } }

		#region Create
		public static string Create { get { return "controltemplatecreate"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region GetAll
		public static string GetAll { get { return "controltemplategetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		public static string GetAllCsv { get { return GetAllApi + "/csv"; } }
		#endregion GetAll

		#region Update
		public static string Update { get { return "controltemplateupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region Delete
		public static string Delete { get { return "controltemplatedelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete

		#region Selector
		public static string RetrieveSelectorApi { get { return UrlApi + "/Selector"; } }
		#endregion Selector
	}
}