using System;
namespace PayIn.Web.App.Factories
{
	public class ServiceCategoryFactory
	{
		public static string UrlApi { get { return "/Api/ServiceCategory"; } }
		public static string Url { get { return "/ServiceCategory"; } }

		#region Get
		public static string Get { get { return UrlApi; } }
		public static string GetApi { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "servicecategorygetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Create
		public static string CreateName { get { return "servicecategorycreate"; } }
		public static string Create { get { return Url; } }
		public static string CreateApi { get { return UrlApi; } } // + "/Create"; } }
		#endregion Create

		#region Delete
		public static string Delete { get { return "servicecategorydelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion
		
		#region Update
		public static string UpdateName { get { return "servicecategoryupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update
				
	}
}