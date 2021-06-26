using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayIn.Web.App.Factories
{
	public partial class TransportValidationFactory
	{
		public static string UrlApi { get { return "/Api/TransportValidation"; } }
		public static string Url { get { return "/TransportValidation"; } }

		#region GetAll
		public static string GetAllName { get { return "transportvalidationgetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		public static string GetAllCsv { get { return GetAllApi + "/csv"; } }
		#endregion GetAll
	}
}
