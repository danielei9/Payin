
namespace PayIn.Web.App.Factories
{
	public partial class ControlPlanningFactory
	{
		public static string UrlApi { get { return "/Api/ControlPlanning"; } }
		public static string Url { get { return "/ControlPlanning"; } }

		#region Clear
		public static string ClearName { get { return "controlplanningclear"; } }
		public static string ClearApi { get { return UrlApi + "/Clear"; } }
		#endregion Clear

		#region Create
		public static string CreateName { get { return "controlplanningcreate"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region CreateTemplate
		public static string CreateTemplateName { get { return "controlplanningcreatetemplate"; } }
		public static string CreateTemplateApi { get { return UrlApi + "/Template"; } }
		#endregion CreateTemplate

		#region Delete
		public static string Delete { get { return "controlplanningdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "controlplanninggetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

	

		#region Update
		public static string Update { get { return "controlplanningupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update
	}
}