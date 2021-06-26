
namespace PayIn.Web.App.Factories
{
	public class ProductFactory
	{
		public static string UrlApi { get { return "/Api/Product"; } }
		public static string Url { get { return "/Product"; } }

		#region Get
		public static string Get { get { return UrlApi; } }
		public static string GetApi { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "productgetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
        #endregion GetAll

        #region Create
        public static string CreateName { get { return "productcreate"; } }
		public static string Create { get { return Url + "/Create/:id"; } }
		public static string CreateApi { get { return UrlApi + "/Create"; } }
		#endregion Create
		
		#region Delete
		public static string Delete { get { return "productdelete"; } }
		public static string DeleteApi { get { return UrlApi + "/:id"; } }
		#endregion Delete

		#region Update
		public static string UpdateName { get { return "productupdate"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
        #endregion Update

        #region RetrieveSelector
        public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
        #endregion RetrieveSelector

        #region CreateImage
        public static string CreateImageName { get { return "productcreateimage"; } }
		public static string CreateImageApi { get { return UrlApi + "/CreateImage/:id"; } }
		#endregion CreateImage

		#region UpdateImageCrop
		public static string UpdateImageCropName { get { return "productimagecrop"; } }
        public static string UpdateImageCrop { get { return UrlApi + "/ImageCrop"; } }
		#endregion UpdateImageCrop

		#region IsVisible
		public static string IsVisibleApi { get { return UrlApi + "/IsVisible/:id"; } }
		#endregion IsVisible

		#region Show
		public static string Show { get { return "productshow"; } }
		#endregion Show

		#region Hide
		public static string Hide { get { return "producthide"; } }
		#endregion Hide

		#region GetVisibility
		public static string GetVisibility { get { return UrlApi; } }
		public static string GetVisibilityApi { get { return UrlApi + "/:id"; } }
		#endregion GetVisibility

		#region Visibility
		public static string VisibilityName { get { return "productvisibility"; } }
		public static string VisibilityApi { get { return UrlApi + "/:id"; } }
		#endregion Visibility

		#region GetAllByDashBoard
		public static string GetAllByDashboardName { get { return "productgetallbydashboard"; } }
		public static string GetAllByDashboard { get { return Url + "/ProductDashboard"; } }
		public static string GetAllByDashboardApi { get { return UrlApi + "/ProductDashboard"; } }
		#endregion GetAllByDashBoard

		#region Groups
		public static string GetAllGroupsName { get { return "productgroupsgetall"; } }
		public static string GetAllGroups { get { return Url + "/Groups/:id"; } }
		public static string GetAllGroupsApi { get { return UrlApi + "/Groups"; } }
		#endregion Groups

		#region AddGroup
		public static string AddGroupName { get { return "productaddgroup"; } }
		//public static string AddGroup { get { return Url + "/Create/:id"; } }
		public static string AddGroup(string id = "") { return AddGroupName + "({id:\"" + id + "\"})"; }
		//public static string AddGroup { get { return Url + "/:id"; } }
		public static string AddGroupApi { get { return UrlApi + "/AddGroup"; } }
		#endregion AddGroup

		#region RemoveGroup
		public static string RemoveGroup { get { return "productremovegroup"; } }
		public static string RemoveGroupApi { get { return UrlApi + "/RemoveGroup/:id"; } }
		#endregion RemoveGroup

		#region GroupsRetrieveSelector
		public static string GroupsRetrieveSelectorApi { get { return UrlApi + "/GroupRetrieveSelector"; } }
		#endregion GroupsRetrieveSelector

	}
}