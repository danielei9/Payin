using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayIn.Web.App.Factories
{
	public partial class GreyListFactory
	{
		public static string UrlApi { get { return "/Api/GreyList"; } }
		public static string Url { get { return "/GreyList"; } }

		#region GetAll
		public static string GetAllName { get { return "greylistgetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		public static string GetAllCsv { get { return GetAllApi + "/csv"; } }
		#endregion GetAll

		#region ReadAll
		public static string ReadAllName { get { return "greylistreadall"; } }
		public static string ReadAll(string Id = "") { return ReadAllName + (Id.IsNullOrEmpty() ? "" : "({id:" + Id + "})"); }
		public static string ReadAllApi { get { return UrlApi + "/ReadAll"; } }
		#endregion ReadAll

		#region ModifyBlock
		public static string ModifyBlockName { get { return "greylistmodifyblock"; } }
		public static string ModifyBlock(string Id = "") { return ModifyBlockName + (Id.IsNullOrEmpty() ? "" : "({id:" + Id + "})"); }
		public static string ModifyBlockApi { get { return UrlApi + "/ModifyBlock"; } }
		#endregion ModifyBlock

		#region ModifyField
		public static string ModifyFieldName { get { return "greylistmodifyfield"; } }
		public static string ModifyField(string Id = "") { return ModifyFieldName + (Id.IsNullOrEmpty() ? "" : "({id:" + Id + "})"); }
		public static string ModifyFieldApi { get { return UrlApi + "/ModifyField"; } }
		#endregion ModifyBlock

		#region Delete
		public static string Delete { get { return "greylistdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion

	}
}
