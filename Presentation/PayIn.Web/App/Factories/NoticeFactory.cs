namespace PayIn.Web.App.Factories
{
	public class NoticeFactory
	{
		public static string Url { get { return "/Notice"; } }
		public static string UrlApi { get { return "/Api/Notice"; } }

		#region Get
		public static string GetApi { get { return UrlApi; } }
		public static string GetApiTemplate { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "noticegetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
        #endregion GetAll

        #region GetPages
        public static string GetPagesName { get { return "noticegetpages"; } }
        public static string GetPages { get { return Url + "/Pages"; } }
        public static string GetPagesApi { get { return UrlApi + "/Pages"; } }
        #endregion GetPages

        #region GetEdicts
        public static string GetEdictsName { get { return "noticegetedicts"; } }
        public static string GetEdicts { get { return Url + "/Edicts"; } }
        public static string GetEdictsApi { get { return UrlApi + "/Edicts"; } }
        #endregion GetEdicts

        #region Create
        public static string CreateName { get { return "noticecreate"; } }
		public static string CreateApi { get { return UrlApi; } }
        #endregion Create

        #region CreatePage
        public static string CreatePageName { get { return "noticecreatepage"; } }
        public static string CreatePageApi { get { return UrlApi + "/Page"; } }
        #endregion CreatePage

        #region CreateEdict
        public static string CreateEdictName { get { return "noticecreateedict"; } }
        public static string CreateEdictApi { get { return UrlApi + "/Edict"; } }
        #endregion CreateEdict

        #region Delete
        public static string Delete { get { return "noticedelete"; } }
		public static string DeleteApi { get { return UrlApi + "/:id"; } }
		#endregion Delete

		#region Update
		public static string UpdateName { get { return "noticeupdate"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
		#endregion Update

		#region UpdatePage
		public static string UpdatePageName { get { return "noticeupdatepage"; } }
        public static string UpdatePageApi { get { return UrlApi + "/Page"; } }
        #endregion UpdatePage

        #region UpdateEdict
        public static string UpdateEdictName { get { return "noticeupdateedict"; } }
        public static string UpdateEdictApi { get { return UrlApi + "/Edict"; } }
        #endregion UpdateEdict

        #region CreateImage
        public static string CreateImageName { get { return "noticecreateimage"; } }
		public static string CreateImageApi { get { return UrlApi + "/CreateImage/:id"; } }
		#endregion CreateImage

		#region UpdateImageCrop
		public static string UpdateImageCropName { get { return "noticeimagecrop"; } }
		public static string UpdateImageCrop { get { return UrlApi + "/ImageCrop"; } }
		#endregion UpdateImageCrop

		#region IsVisible
		public static string IsVisibleApi { get { return UrlApi + "/IsVisible/:id"; } }
		#endregion IsVisible

		#region Show
		public static string Show { get { return "noticeshow"; } }
		#endregion Show

		#region Hide
		public static string Hide { get { return "noticehide"; } }
		#endregion Hide

		#region GetVisibility
		public static string GetVisibility { get { return UrlApi; } }
		public static string GetVisibilityApi { get { return UrlApi + "/:id"; } }
		#endregion GetVisibility

		#region Visibility
		public static string VisibilityName { get { return "noticevisibility"; } }
		public static string VisibilityApi { get { return UrlApi + "/:id"; } }
		#endregion Visibility

		#region RetrieveSelector
		public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
		#endregion RetrieveSelector
	}
}