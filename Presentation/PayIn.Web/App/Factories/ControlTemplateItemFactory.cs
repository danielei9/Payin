namespace PayIn.Web.App.Factories
{
	public partial class ControlTemplateItemFactory
	{
		public static string UrlApi { get { return "/Api/ControlTemplateItem"; } }
		public static string Url { get { return "/ControlTemplateItem"; } }

		#region Create
		public static string Create { get { return "controltemplateitemcreate"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region GetTemplate
		public static string GetTemplate { get { return "controltemplateitemgettemplate"; } }
		public static string GetTemplateApi { get { return UrlApi + "/template"; } }
		public static string GetTemplateCsv { get { return GetTemplateApi + "/csv"; } }
		#endregion GetTemplate

		#region Update
		public static string Update { get { return "controltemplateitemupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region Delete
		public static string Delete { get { return "controltemplateitemdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete

		#region Selector
		public static string RetrieveSelectorApi { get { return UrlApi + "/Selector"; } }
		#endregion Selector
	}
}