using System;
namespace PayIn.Web.App.Factories
{
	public class ServiceGroupFactory
	{
		public static string UrlApi { get { return "/Api/ServiceGroup"; } }
		public static string Url { get { return "/ServiceGroup"; } }

		#region Get
		public static string Get { get { return UrlApi; } }
		public static string GetApi { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "servicegroupgetall"; } }
		public static string GetAll { get { return Url; } } //(string GroupId = "") { return GetAllName + (GroupId.IsNullOrEmpty() ? "" : "({GroupId:" + GroupId + "})"); }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Create
		public static string CreateName { get { return "servicegroupcreate"; } }
		public static string Create { get { return Url + "/Create/:id"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Delete
		public static string Delete { get { return "servicegroupdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion
		
		#region Update
		public static string UpdateName { get { return "servicegroupupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update
				
		#region RetrieveSelector
		public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
		#endregion RetrieveSelector

		#region AddUser
		public static string AddUserName { get { return "servicegroupadduser"; } }
		//public static string AddUser { get { return Url + "/AddUser"; } }
		public static string AddUser(string groupId = "") { return AddUserName + ("({id:\"" + groupId + "\"})"); }
		public static string AddUserApi { get { return UrlApi + "/AddUser"; } }
		#endregion AddUser
	}
}