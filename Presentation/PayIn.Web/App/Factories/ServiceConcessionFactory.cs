
namespace PayIn.Web.App.Factories
{
	public class ServiceConcessionFactory
	{
		public static string UrlApi { get { return "/Api/ServiceConcession"; } }
		public static string Url { get { return "/ServiceConcession"; } }

		#region Get
		public static string GetApi { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "serviceconcessiongetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region RetrieveSelector
		public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
		#endregion RetrieveSelector

		#region ConcessionsRetrieveSelector
		public static string ConcessionsRetrieveSelectorApi { get { return UrlApi + "/Concessions/RetrieveSelector"; } }
		#endregion ConcessionsRetrieveSelector

		#region Update
		public static string UpdateName { get { return "serviceconcessionupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region UpdateCommerce
		public static string UpdateCommerce { get { return "serviceconcessionupdatecommerce"; } }
		public static string UpdateCommerceApi { get { return UrlApi +"/UpdateCommerce"; } }
		#endregion UpdateCommerce

		#region GetCommerce
		public static string GetCommerce { get { return "serviceconcessiongetcommerce"; } }
		public static string GetCommerceApi { get { return UrlApi; } }
		#endregion GetCommerce

		#region Delete
		public static string Delete { get { return "serviceconcessiondelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete
	}
}
