
namespace PayIn.Web.App.Factories
{
	public partial class AccountFactory
	{
		public static string UrlApi { get { return "/Api/Account"; } }
		//public static string Url { get { return "/Account"; } }

		//#region Create
		//public static string Create { get { return "controlitemcreate"; } }
		//public static string CreateApi { get { return UrlApi; } }
		//#endregion Create

		//#region Get
		//public static string GetApi { get { return UrlApi; } }
		//#endregion Get

		#region GetCurrent
		public static string GetCurrent { get { return "accountcurrent"; } }
		public static string GetCurrentApi { get { return UrlApi + "/Current"; } }
		#endregion GetCurrent

		#region GetUsers
		public static string GetUsers { get { return "accountusers"; } }
		public static string GetUsersApi { get { return UrlApi + "/GetUsers"; } }
		#endregion GetUSers

		#region OverWritePassword
		public static string OverwritePasswordName { get { return "accountoverwritepassword"; } }
		public static string OverwritePassword(string userId) { return OverwritePasswordName + "({id:" + userId + "})"; }
		public static string OverwritePasswordApi { get { return UrlApi + "/OverwritePassword"; } }
		#endregion OverWritePassword

		#region UnlockUser
		public static string UnlockUserName { get { return "accountunlockuser"; } }
		public static string UnlockUser(string userId) { return UnlockUserName + "({id:" + userId + "})"; }
		public static string UnlockUserApi { get { return UrlApi + "/UnlockUser"; } }
		#endregion OverWritePassword

		//#region GetAll
		//public static string GetAll { get { return "controlitemgetall"; } }
		//public static string GetAllApi { get { return UrlApi; } }
		//public static string GetAllCsv { get { return GetAllApi + "/csv"; } }
		//#endregion GetAll

		#region Update
		//public static string Update { get { return "controlitemupdate"; } }
		//public static string UpdateApi { get { return UrlApi; } }
		public static string UpdatePassword { get { return "/Account/UpdatePassword"; } }
		#endregion Update

		//#region Delete
		//public static string Delete { get { return "controlitemdelete"; } }
		//public static string DeleteApi { get { return UrlApi; } }
		//#endregion Delete

		//#region Selector
		//public static string RetrieveSelectorApi { get { return UrlApi + "/Selector"; } }
        //#endregion Selector

        #region UserAccountSettings
        public static string UpdateSettingsName { get { return "accountupdate";} }
        public static string UpdateSettings { get { return UrlApi; } }    
        #endregion UserAccountSettings

		//#region UserAccountPhotoSettings
		//public static string UpdatePhotoName { get { return "accountuserphotosetting"; } }
		//public static string UpdatePhoto { get { return UrlApi + "/UserPhoto"; } }
		//#endregion UserAccountPhotoSettings

		#region ImageCrop
		public static string UpdateImageCropName { get { return "accountimagecrop"; } }
		public static string UpdateImageCrop { get { return UrlApi + "/ImageCrop"; } }
		#endregion ImageCrop


		
		#region DeleteImage
		public static string DeleteImageName { get { return "accountdeleteimage"; } }
		public static string DeleteImageApi { get { return UrlApi + "/DeleteImage"; } }
		#endregion DeleteImage

	}
}