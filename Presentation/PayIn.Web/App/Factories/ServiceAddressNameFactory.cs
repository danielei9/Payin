
namespace PayIn.Web.App.Factories
{
	public partial class ServiceAddressNameFactory
	{
		public static string UrlApi { get { return "/Api/ServiceAddressName"; } }
		public static string Url { get { return "/ServiceAddressName"; } }


		#region Create
		public static string Create { get { return "serviceaddresscreatename"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Update
		public static string UpdateName { get { return "UpdateName"; } }
		public static string Update { get { return "serviceaddressupdatename"; } }
		public static string UpdateApi { get { return UrlApi; } }
		public static string UpdateUri { get { return ServiceAddressFactory.RetrieveAll + "#" + UpdateName; } }
		#endregion Update

		#region Delete
		public static string Delete { get { return "serviceaddressdeletename"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete

		//#region Create
		//public static string CreateName { get { return "CreateName"; } }
		//public static string CreateApi { get { return UrlApi; } }
		//public static string CreateUri { get { return ServiceAddressFactory.RetrieveAll + "#" + CreateName; } }
		//#endregion Create

		//#region Delete
		//public static string DeleteName { get { return "DeleteName"; } }
		//public static string DeleteApi { get { return UrlApi; } }
		//public static string DeleteUri { get { return ServiceAddressFactory.RetrieveAll + "#" + DeleteName; } }
		//#endregion Delete

		//#region Update
		//public static string UpdateName { get { return "UpdateName"; } }
		//public static string UpdateApi { get { return UrlApi; } }
		//public static string UpdateUri { get { return ServiceAddressFactory.RetrieveAll + "#" + UpdateName; } }
		//#endregion Update
	}
}
