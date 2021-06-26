
namespace PayIn.Web.App.Factories
{
	public partial class ControlItemFactory
	{
		public static string UrlApi { get { return "/Api/ControlItem"; } }
		public static string Url { get { return "/ControlItem"; } }

		#region Create
		public static string Create { get { return "controlitemcreate"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region Name
		public static string NameApi { get { return UrlApi + "/Name"; } }
		#endregion Name

		#region Legacy
		public static string Legacy  { get { return "controlitemlegacy"; } }
		#endregion Legacy

		#region GetAll
		public static string GetAll { get { return "controlitemgetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		public static string GetAllCsv { get { return GetAllApi + "/csv"; } }
		#endregion GetAll

		#region Update
		public static string Update { get { return "controlitemupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region AddTag
		public static string AddTag { get { return "controlitemaddtag"; } }
		public static string AddTagApi { get { return UrlApi + "/AddTag"; } }
		#endregion AddTag

		#region RemoveTag
		public static string RemoveTag { get { return "controlitemremovetag"; } }
		public static string RemoveTagApi { get { return UrlApi + "/RemoveTag"; } }
		#endregion RemoveTag

		#region Delete
		public static string Delete { get { return "controlitemdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete

		#region Selector
		public static string RetrieveSelectorApi { get { return UrlApi + "/Selector"; } }
		#endregion Selector
	}
}