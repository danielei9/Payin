using System;

namespace PayIn.Web.App.Factories
{
	public partial class ControlFormAssignCheckPointFactory
	{
		public static string UrlApi { get { return "/Api/ControlFormAssignCheckPoint"; } }
		public static string Url { get { return "/ControlFormAssignCheckPoint"; } }

		#region GetCheck
		public static string GetCheckStateName { get { return "controlformassigncheckpointgetcheck"; } }
		public static string GetCheckState { get { return GetCheckStateName; } }
		public static string GetCheckApi { get { return UrlApi; } }
		#endregion GetCheck

		#region Get
		public static string GetName { get { return "controlformassigncheckpointget"; } }
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region Create
		public static string CreateStateName { get { return "controlformassigncheckpointcreate"; } }
		public static string CreateState(string checkId = "") { return CreateStateName + (checkId.IsNullOrEmpty() ? "" : "({checkId:" + checkId + "})"); }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		//#region Update
		//public static string UpdateStateName { get { return "controlformassigncheckpointupdate"; } }
		//public static string UpdateState(string id) { return UpdateStateName + "({id:" + id + "})"; }
		//public static string UpdateApi { get { return UrlApi; } }
		//public static string UpdateGetApi { get { return UrlApi; } }
		//#endregion Update

		#region Delete
		public static string DeleteStateName { get { return "controlformassigncheckpointdelete"; } }
		public static string DeleteState(string id) { return DeleteStateName + "({id:" + id + "})"; }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete
	}
}