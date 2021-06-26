
namespace PayIn.Web.App.Factories
{
	public partial class ControlPlanningCheckFactory
	{
		public static string UrlApi { get { return "/Api/ControlPlanningCheck"; } }
		public static string Url { get { return "/ControlPlanningCheck"; } }

		#region Create
		public static string CreateName { get { return "controlplanningcheckcreate"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Delete
		public static string DeleteState { get { return "controlplanningcheckdelete"; } }

		public static string DeleteUrl { get { return Url + "/Delete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region Update
		public static string Update { get { return "controlplanningcheckupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update
	}
}