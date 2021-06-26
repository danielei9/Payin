
namespace PayIn.Web.App.Factories
{
	public partial class ServiceSupplierFactory
	{
		public static string UrlApi { get { return "/Api/ServiceSupplier"; } }
		public static string Url { get { return "/ServiceSupplier"; } }
		
		#region GetSelector
		public static string GetSelectorApi { get { return UrlApi + "/Selector"; } }
		#endregion GetSelector

		#region GetAll 
		public static string GetAllSuppliersName { get { return "servicesuppliergetall"; } }
		public static string GetAllSuppliers { get { return UrlApi; } }
		public static string GetAllSuppliersApi { get { return UrlApi; } }
		#endregion GetAll

		#region Update
		public static string UpdateName { get { return "servicesupplierupdate"; } }
		public static string UpdaterApi { get { return UrlApi; } }
		#endregion Update

		#region GetConcession
		public static string ConcessionGetApi { get { return UrlApi + "/Concession"; } }
		#endregion GetConcession

	}
}
