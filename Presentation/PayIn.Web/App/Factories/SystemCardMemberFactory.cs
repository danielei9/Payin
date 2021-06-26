namespace PayIn.Web.App.Factories
{
	public partial class SystemCardMemberFactory
    {
		public static string UrlApi { get { return "/Api/SystemCardMember"; } }
		public static string Url { get { return "/SystemCardMember"; } }

        #region GetAll
        public static string GetAllName{ get { return "systemcardmembergetall"; } }
        public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Get
		//public static string GetSupplierApi { get { return "/Api/ServiceSupplier/Current"; } }
		//public static string GetName { get { return "systemcardmemberget"; } }
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region Create
		public static string CreateName { get { return "systemcardmembercreate"; } }
        public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Delete
		public static string DeleteName { get { return "systemcardmemberdelete"; } }
		public static string DeleteApi { get { return UrlApi + "/:id"; } }
		#endregion Delete

		#region Update
		//public static string UpdateName => "systemcardmemberupdate";
		public static string UpdateName { get { return "systemcardmemberupdate"; } }
		//public static string Update { get { return Url + "/Update"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
		#endregion Update

		#region LockSystemCardMember
		public static string LockSystemCardMemberName { get { return "systemcardmemberlock"; } }
		public static string LockSystemCardMemberApi { get { return UrlApi + "/Lock/:id"; } }
		#endregion LockSystemCardMember

		#region UnlockSystemCardMember
		public static string UnlockSystemCardMemberName { get { return "systemcardmemberunlock"; } }
		public static string UnlockSystemCardMemberApi { get { return UrlApi + "/Unlock/:id"; } }
		#endregion UnlockSystemCardMember

	}
}