
namespace PayIn.Web.App.Factories
{
	public partial class ControlPlanningItemFactory
	{
		public static string UrlApi { get { return "/Api/ControlPlanningItem"; } }
		public static string Url { get { return "/ControlPlanningItem"; } }

		#region Create
		public static string CreateName { get { return "controlplanningitemcreate"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region Update
		public static string Update { get { return "controlplanningitemupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region Delete
		public static string Delete { get { return "controlplanningitemdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete
	}
}