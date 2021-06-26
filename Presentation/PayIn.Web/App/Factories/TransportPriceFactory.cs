using System;

namespace PayIn.Web.App.Factories
{
	public partial class TransportPriceFactory
	{
		public static string UrlApi { get { return "/Api/TransportPrice"; } }		
		public static string Url { get { return "/TransportPrice"; } }		

		#region GetAll
		public static string GetAllName { get { return "transportpricegetall"; } }
		public static string GetAll(string titleId = "") { return GetAllName + (titleId.IsNullOrEmpty() ? "" : "({titleId:" + titleId + "})"); }
		//public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region Create
		public static string CreateName { get { return "transportpricecreate"; } }
		public static string Create(string titleId = "") { return CreateName + (titleId.IsNullOrEmpty() ? "" : "({titleId:" + titleId + "})"); }
		//public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Update
		public static string UpdateName { get { return "transportpriceupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region Delete
		public static string Delete { get { return "transportpricedelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion

		#region Selector
		public static string RetrieveSelectorApi { get { return UrlApi + "/Selector"; } }
		#endregion Selector

	}
}
