using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayIn.Web.App.Factories
{
	public partial class ControlTemplateCheckFactory
	{
		public static string UrlApi { get { return "/Api/ControlTemplateCheck"; } }
		public static string Url { get { return "/ControlTemplateCheck"; } }

		#region Create
		public static string CreateName { get { return "controltemplatecheckcreate"; } }
		public static string CreateUrl { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Delete
		public static string DeleteState { get { return "controltemplatecheckdelete"; } }
		public static string DeleteUrl { get { return Url + "/Delete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete

		#region Get
		public static string GetName { get { return "controltemplatecheckget"; } }
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region GetTemplate
		public static string GetTemplate { get { return "controltemplatecheckgettemplate"; } }
		public static string GetTemplateApi { get { return UrlApi + "/template"; } }
		#endregion GetTemplate

		#region Update
		public static string Update { get { return "controltemplatecheckupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region GetAll
		public static string GetAllName { get { return "controltemplatecheckgetall"; } }
		public static string GetAllUrl { get { return Url + "/GetAll"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion Get
	}
}