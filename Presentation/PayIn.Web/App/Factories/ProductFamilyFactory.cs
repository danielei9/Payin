namespace PayIn.Web.App.Factories
{
	public class ProductFamilyFactory
	{
		public static string UrlApi { get { return "/Api/ProductFamily"; } }
		public static string Url { get { return "/ProductFamily"; } }

		#region Get
		public static string Get{ get { return UrlApi; } }
		public static string GetApi { get { return UrlApi + "/:id"; } }
		#endregion Get

        #region GetAll
        public static string GetAllName { get { return "productgetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Create
        public static string CreateName { get { return "productfamilycreate"; } }
        public static string Create { get { return Url + "/Create/:id"; } }
        public static string CreateApi { get { return UrlApi + "/Create"; } }
        #endregion Create

        #region Delete
        public static string Delete { get { return "productfamilydelete"; } }
        public static string DeleteApi { get { return UrlApi + "/:id"; } }
        #endregion Delete

        #region Update
        public static string UpdateName { get { return "productfamilyupdate"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
        #endregion Update

        #region RetrieveSelector
        public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
        #endregion RetrieveSelector

        #region CreateImage
        public static string CreateImageName { get { return "productcreateimage"; } }
        public static string CreateImageApi { get { return UrlApi + "/CreateImage/:id"; } }

        public static string CreateFamilyImageName { get { return "productfamilycreateimage"; } }
        public static string CreateFamilyImageApi { get { return CreateFamilyImageName + "/CreateImage/:id"; } }
        #endregion CreateImage

        #region UpdateImageCrop
        public static string UpdateImageCropName { get { return "productfamilyimagecrop"; } }
        public static string UpdateImageCrop { get { return UrlApi + "/ImageCrop"; } }
		#endregion UpdateImageCrop

		#region IsVisible
		public static string IsVisibleApi { get { return UrlApi + "/IsVisible/:id"; } }
		#endregion IsVisible

		#region Show
		public static string Show { get { return "productfamilyshow"; } }
		#endregion Show

		#region Hide
		public static string Hide { get { return "productfamilyhide"; } }
		#endregion Hide
	}
}