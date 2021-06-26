namespace PayIn.Web.App.Factories
{
	public class ServiceDocumentFactory
	{
		public static string UrlApi { get { return "/Api/ServiceDocument"; } }
		public static string Url { get { return "/ServiceDocument"; } }

		#region Get
		public static string Get { get { return UrlApi; } }
		public static string GetApi { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "servicedocumentgetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Create
		public static string CreateName { get { return "servicedocumentcreate"; } }
		public static string Create { get { return Url + "/Create/:id"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create
		
		#region GetCreate
		public static string GetCreateApi { get { return UrlApi + "/GetCreate"; } }
		#endregion GetCreate

		#region Delete
		public static string DeleteName { get { return "servicedocumentdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion
		
		#region Update
		public static string UpdateName { get { return "servicedocumentupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update
	}
}