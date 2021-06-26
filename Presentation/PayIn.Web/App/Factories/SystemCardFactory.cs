namespace PayIn.Web.App.Factories
{
	public class SystemCardFactory
	{
		public static string UrlApi { get { return "/Api/SystemCard"; } }
		public static string Url { get { return "/SystemCard"; } }

		#region GetAll
		public static string GetAllName { get { return "systemcardgetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Get
		public static string GetName { get { return "systemcardget"; } }
		public static string GetApi { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region Create
		public static string CreateName { get { return "systemcardcreate"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Update
		public static string UpdateName { get { return "systemcardupdate"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
		#endregion Update

		#region Delete
		public static string DeleteName { get { return "systemcarddelete"; } }
		public static string DeleteApi { get { return UrlApi + "/:id"; } }
		#endregion Delete

		#region RetrieveSelector
		public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
		#endregion RetrieveSelector

		#region UpdateImageCrop
		public static string UpdateImageCropName { get { return "systemcardimagecrop"; } }
		public static string UpdateImageCrop { get { return UrlApi + "/ImageCrop"; } }
		#endregion UpdateImageCrop
	}
}