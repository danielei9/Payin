
namespace PayIn.Web.App.Factories
{
	public partial class ServicePriceFactory
	{
		public static string UrlApi { get { return "/Api/ServicePrice"; } }
		public static string Url { get { return "/ServicePrice"; } }

		#region Create
		public static string Create { get { return "servicepricecreate"; } }
		public static string CreateApi { get { return UrlApi; } }

		#endregion Create

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region GetAll
		public static string GetAll { get { return "servicepricegetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		public static string GetAllCsv { get { return GetAllApi + "/csv"; } }
		#endregion GetAll

		#region Update
		public static string Update { get { return "servicepriceupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region Delete
		public static string Delete { get { return "servicepricedelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete
	}
}
