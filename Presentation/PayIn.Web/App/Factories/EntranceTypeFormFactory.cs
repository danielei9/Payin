using System;

namespace PayIn.Web.App.Factories
{
	public class EntranceTypeFormFactory
	{
		public static string UrlApi { get { return "/Api/EntranceTypeForm"; } }
		public static string Url { get { return "/EntranceTypeForm"; } }

		#region GetAll
		public static string GetAllName { get { return "entrancetypeformgetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Get
		public static string GetApi { get { return UrlApi; } }
		public static string GetApiTemplate { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region Create
		public static string CreateName { get { return "entrancetypeformcreate"; } }
        public static string CreateCall(string id) { return CreateName + "({\"entranceTypeId\":\"" + id + "\"})"; }
        public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Update
		public static string UpdateName { get { return "entrancetypeformupdate"; } }
		public static string Update { get { return Url + "/Update"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
		#endregion Update

		#region Delete
		public static string Delete { get { return "entrancetypeformdelete"; } }
		public static string DeleteApi { get { return UrlApi + "/:id"; } }
		#endregion Delete

		#region RetrieveSelector
		public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
		#endregion RetrieveSelector
	}
}