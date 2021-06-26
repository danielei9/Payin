using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayIn.Web.App.Factories
{
	public partial class PromotionFactory
	{
		public static string UrlApi { get { return "/Api/Promotion"; } }
		public static string Url { get { return "/Promotions"; } }

		#region GetAll
		public static string GetAllName { get { return "promotiongetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		public static string GetAllCsv { get { return GetAllApi + "/csv"; } }
		#endregion

		#region GetAllConcession
		public static string GetAllConcessionName { get { return "promotiongetallconcession"; } }
		public static string GetAllConcession { get { return Url; } }
		public static string GetAllConcessionApi { get { return UrlApi + "/GetAllConcession"; } }
		public static string GetAllConcessionCsv { get { return GetAllConcessionApi + "/csv"; } }
		#endregion

		#region Create
		public static string CreateName { get { return "promotioncreate"; } }
		public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi + "/Create"; } }
		#endregion		

		#region Delete
		public static string Delete { get { return "promotiondelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion

		#region UnlinkCode
		public static string UnlinkCode { get { return "promotionunlinkcode"; } }
		public static string UnlinkCodeApi { get { return UrlApi + "/UnlinkCode"; } }
		#endregion

		#region ViewCode
		public static string GetCodeName { get { return "promotionviewcode"; } }
		public static string GetCode(string Id = "") { return GetCodeName + (Id.IsNullOrEmpty() ? "" : "({id:" + Id + "})"); }

		public static string GetCodeApi { get { return UrlApi + "/ViewCode"; } }
		#endregion ViewCode

		#region AddCode
		public static string AddCodeName { get { return "promotionaddcode"; } }
		public static string AddCode(string Id = "") { return AddCodeName + (Id.IsNullOrEmpty() ? "" : "({id:" + Id + "})"); }
		public static string AddCodeApi { get { return UrlApi + "/AddCode"; } }
		#endregion AddCode

		#region Update
		public static string UpdateName { get { return "promotionupdate"; } }
		public static string Update { get { return Url + "/Update"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
		#endregion Update
	}
}
