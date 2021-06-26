using System;

namespace PayIn.Web.App.Factories
{
	public class ControlFormAssignTemplateFactory
	{
		public static string UrlApi { get { return "/Api/ControlFormAssignTemplate"; } }
		public static string Url { get { return "/ControlFormAssignTemplate"; } }

		#region GetCheck
		public static string GetCheckStateName { get { return "controlformassigntemplategetcheck"; } }
		public static string GetCheckState { get { return GetCheckStateName; } }
		public static string GetCheckApi { get { return UrlApi; } }
		#endregion GetCheck

		#region Create
		public static string CreateStateName { get { return "controlformassigntemplatecreate"; } }
		public static string CreateState(string checkId = "") { return CreateStateName + (checkId.IsNullOrEmpty() ? "" : "({checkId:" + checkId + "})"); }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Update
		public static string UpdateStateName { get { return "controlformassigntemplateupdate"; } }
		public static string UpdateState(string id) { return UpdateStateName + "({id:" + id + "})"; }
		public static string UpdateApi { get { return UrlApi; } }
		public static string UpdateGetApi { get { return UrlApi; } }
		#endregion Update

		#region Delete
		public static string DeleteStateName { get { return "controlformassigntemplatedelete"; } }
		public static string DeleteState(string id) { return DeleteStateName + "({id:" + id + "})"; }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete
	}
}