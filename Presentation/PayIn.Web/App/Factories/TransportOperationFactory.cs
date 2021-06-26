using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayIn.Web.App.Factories
{
	public partial class TransportOperationFactory
	{
		public static string UrlApi { get { return "/Api/Transport"; } }		
		public static string Url { get { return "/Transport"; } }		

		#region GetAll
		public static string GetAllName { get { return "transportoperationgetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		public static string GetAllCsv { get { return GetAllApi + "/csv"; } }
		#endregion GetAll
			
		#region Details
		public static string DetailsName { get { return "transportoperationdetails"; } }
		public static string DetailsOperationId(string id = "", string uid = "") { return DetailsName + "({" + (id.IsNullOrEmpty() ? "" : "id:" + id) + (uid.IsNullOrEmpty() ? "" : "," + "uid:" + uid) + "})"; }
		public static string Details(string id = "", string uid = "") { return DetailsName + "({" + (id.IsNullOrEmpty() ? "" : "id:" + id) + (uid.IsNullOrEmpty() ? "" : "," + "uid:" + uid) + "})"; }
		public static string DetailsApi { get { return UrlApi + "/Details"; } }
		#endregion Details
	}
}
