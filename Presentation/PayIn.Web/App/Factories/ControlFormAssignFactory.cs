using System;

namespace PayIn.Web.App.Factories
{
	public partial class ControlFormAssignFactory
	{
		public static string UrlApi { get { return "/Api/ControlFormAssign"; } }
		public static string Url { get { return "/ControlFormAssign"; } }

		#region GetCheck
		public static string GetCheckStateName { get { return "controlformassigngetcheck"; } }
		public static string GetCheckState(string checkId) { return GetCheckStateName + "({checkId:" + checkId + "})"; }
		public static string GetCheckUrl(string checkId) { return Url + "/Check/" + checkId; }
		public static string GetCheckApi { get { return UrlApi; } }
		#endregion GetCheck

		#region GetForm
		public static string GetFormStateName { get { return "controlformassignform"; } }
		public static string GetFormState(string checkId) { return GetFormStateName + "({checkId:" + checkId + "})"; }
		public static string GetFormUrl(string checkId) { return Url + "/ViewCompleteForm/" + checkId; }
		public static string GetFormApi { get { return UrlApi; } }
		#endregion GetCheck

		#region Create
		public static string CreateStateName { get { return "controlformassigncreate"; } }
		public static string CreateState(string checkId = "") { return CreateStateName + (checkId.IsNullOrEmpty() ? "" : "({checkId:" + checkId + "})"); }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Update
		public static string UpdateStateName { get { return "controlformassignupdate"; } }
		public static string UpdateState(string id) { return UpdateStateName + "({id:" + id + "})"; }
		public static string UpdateApi { get { return UrlApi; } }
		public static string UpdateGetApi { get { return UrlApi + "/Ids"; } }
		#endregion Update

		#region Delete
		public static string DeleteStateName { get { return "controlformassigndelete"; } }
		public static string DeleteState(string id) { return DeleteStateName + "({id:" + id + "})"; }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete
	}
}