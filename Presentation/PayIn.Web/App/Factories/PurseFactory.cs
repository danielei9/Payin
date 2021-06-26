namespace PayIn.Web.App.Factories
{
	public class PurseFactory
	{
		public static string UrlApi { get { return "/Api/Purse"; } }
		public static string Url { get { return "/Purse"; } }

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "pursegetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region GetUsers
		public static string GetUsersName { get { return "pursegetusers"; } }
		public static string GetUsers { get { return Url +"/Users"; } }
		public static string GetUsersApi { get { return UrlApi +"/Users/:id"; } }
		#endregion GetUsers

		#region Create
		public static string CreateName { get { return "pursecreate"; } }
		public static string CreateApi { get { return UrlApi; } }
        #endregion Create

        #region CreateImage
		public static string CreateImageName { get { return "pursecreateimage"; } }
		public static string CreateImageApi { get { return UrlApi + "/CreateImage/:id"; } }
        #endregion CreateImage

        #region Delete
        public static string Delete { get { return "pursedelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
        #endregion

		#region DeleteImage
		public static string DeleteImageName { get { return "pursedeleteimage"; } }
		public static string DeleteImageApi { get { return UrlApi + "/DeleteImage/:id"; } }
		#endregion DeleteImage

        #region ImageCrop
        public static string UpdateImageCropName { get { return "purseimagecrop"; } }
		public static string CreateImageCropName { get { return "purseimagecropcreate"; } }
        public static string UpdateImageCrop { get { return UrlApi + "/ImageCrop"; } }
        #endregion ImageCrop

        #region Update
        public static string UpdateName { get { return "purseupdate"; } }
		public static string Update { get { return Url + "/Update"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
		#endregion Update

		#region retrieveSelectorBySystemCard
		public static string PurseBySystemCardRetrieveSelectorApi { get { return UrlApi + "/BySystemCardRetrieveSelector"; } }
		#endregion retrieveSelectorBySystemCard
	}
}
