namespace PayIn.Web.App.Factories
{
	public class EventFactory
	{
		public static string UrlApi { get { return "/Api/Event"; } }
		public static string Url { get { return "/Event"; } }

		#region Get
		public static string Get { get { return UrlApi; } }
		public static string GetApi { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region GetVisibility
		public static string GetVisibility { get { return UrlApi; } }
		public static string GetVisibilityApi { get { return UrlApi + "/:id"; } }
		#endregion GetVisibility

		#region GetAll
		public static string GetAllName { get { return "eventgetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Create
		public static string CreateName { get { return "eventcreate"; } }
		public static string Create { get { return Url + "/Create/:id"; } }
        public static string CreateApi { get { return UrlApi; } }
        public static string CreateGetApi { get { return UrlApi + "/Create"; } }
        #endregion Create

        #region Delete
        public static string Delete { get { return "eventdelete"; } }
		public static string DeleteApi { get { return UrlApi + "/:id"; } }
		#endregion Delete

		#region Update
		public static string UpdateName { get { return "eventupdate"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
		#endregion Update

		#region CreateImage
		public static string CreateImageName { get { return "eventcreateimage"; } }
		public static string CreateImageApi { get { return UrlApi + "/CreateImage/:id"; } }
		#endregion CreateImage

		#region UpdateImageCrop
		public static string UpdateImageCropName { get { return "eventimagecrop"; } }
		public static string UpdateImageCrop { get { return UrlApi + "/ImageCrop"; } }
        #endregion UpdateImageCrop

		#region UpdateImageMenuCrop
		public static string UpdateImageMenuCropName { get { return "eventimagemenucrop"; } }
		public static string UpdateImageMenuCrop { get { return UrlApi + "/ImageMenuCrop"; } }
        #endregion UpdateImageMenuCrop

        #region CreateMapImage
        public static string CreateMapImageName { get { return "eventcreatemapimage"; } }
        public static string CreateMapImageApi { get { return UrlApi + "/CreateMapImage/:id"; } }
        #endregion CreateMapImage

        #region UpdateMapImageCrop
        public static string UpdateMapImageCropName { get { return "eventmapimagecrop"; } }
        public static string UpdateMapImageCrop { get { return UrlApi + "/MapImageCrop"; } }
        #endregion UpdateMapImageCrop

        #region Suspend
        public static string SuspendName { get { return "eventsuspend"; } }
        public static string SuspendApi { get { return UrlApi + "/Suspend"; } }
        #endregion Suspend

        #region Unsuspend
        public static string UnsuspendName { get { return "eventunsuspend"; } }
        public static string UnsuspendApi { get { return UrlApi + "/Unsuspend"; } }
        #endregion Unsuspend

        #region RetrieveSelector
        public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
		public static string ProfileRetrieveSelectorApi { get { return UrlApi + "/ProfileRetrieveSelector"; } }
		#endregion RetrieveSelector

		#region IsVisible
		public static string IsVisibleApi { get { return UrlApi + "/IsVisible/:id"; } }
		#endregion IsVisible

		#region Show
		public static string Show { get { return "eventshow"; } }
		#endregion Show

		#region Hide
		public static string Hide { get { return "eventhide"; } }
		#endregion Hide

		#region Visibility
		public static string VisibilityName { get { return "eventvisibility"; } }
		public static string VisibilityApi { get { return UrlApi + "/:id"; } }
		#endregion Visibility

		#region AddImageGallery
		public static string AddImageGalleryName { get { return "eventaddimagegallery"; } }
        public static string AddImageGalleryApi { get { return UrlApi + "/AddImage"; } }
        #endregion AddImageGallery

        #region Unsuspend
        public static string DeleteImageGalleryName { get { return "eventdeleteimagegallery"; } }
        public static string DeleteImageGalleryApi { get { return UrlApi + "/DeleteImage"; } }
        #endregion Unsuspend
    }
}