using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayIn.Web.App.Factories
{
	public partial class BlackListFactory
	{
		public static string UrlApi { get { return "/Api/BlackList"; } }
		public static string Url { get { return "/BlackList"; } }

		#region GetAll
		public static string GetAllName { get { return "blacklistgetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		public static string GetAllCsv { get { return GetAllApi + "/csv"; } }
		#endregion GetAll

		#region Create
		public static string CreateName { get { return "blacklistcreate"; } }
		public static string Create { get { return Url; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Delete
		public static string DeleteName { get { return "blacklistdelete"; } }
		public static string Delete { get { return Url; } }
		public static string DeleteApi { get { return UrlApi + "/Delete"; } }
		#endregion Delete

	}
}
