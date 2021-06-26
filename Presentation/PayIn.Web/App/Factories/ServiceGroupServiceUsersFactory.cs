using System;
namespace PayIn.Web.App.Factories
{
	public class ServiceGroupServiceUsersFactory
	{
		public static string UrlApi { get { return "/Api/ServiceGroupServiceUsers"; } }
		public static string Url { get { return "/ServiceGroupServiceUsers"; } }

		#region Get
		public static string Get{ get { return UrlApi; } }
		public static string GetApi { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "servicegroupserviceusersgetall"; } }
		public static string GetAll(string groupId = "") { return GetAllName + (groupId.IsNullOrEmpty() ? "" : "({groupId:" + groupId + "})"); }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Create
		public static string CreateName { get { return "servicegroupserviceuserscreate"; } }
		public static string Create(string userId = "") { return CreateName + (userId.IsNullOrEmpty() ? "" : "({userId:" + userId + "})"); }
		public static string CreateApi { get { return UrlApi + "/Create"; } }
		public static string CreateGetNameApi { get { return UrlApi + "/Create"; } }

		#endregion Create

		#region Delete
		public static string Delete { get { return "servicegroupserviceusersdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion

		#region Update
		public static string UpdateName { get { return "servicegroupserviceusersupdate"; } }
		public static string UpdateApi { get { return UrlApi + "/Update"; } }
		#endregion Update

		#region RetrieveSelector
		public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
		#endregion RetrieveSelector

	}
}