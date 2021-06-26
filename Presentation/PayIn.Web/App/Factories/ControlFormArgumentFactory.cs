using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayIn.Web.App.Factories
{
	public class ControlFormArgumentFactory
	{
		public static string UrlApi { get { return "/Api/ControlFormArgument"; } }
		public static string Url { get { return "/ControlFormArgument"; } }

		#region GetAll
		public static string GetFormStateName { get { return "controlformargumentgetform"; } }
		public static string GetFormState(string formId = "") { return GetFormStateName + (formId.IsNullOrEmpty() ? "" : "({formId:" + formId + "})"); }
		public static string GetFormApi { get { return UrlApi + "/Form"; } }
		#endregion GetAll

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region Create
		public static string CreateName { get { return "controlformargumentcreate"; } }
		public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Update
		public static string UpdateName { get { return "controlformargumentupdate"; } }
		public static string Update { get { return Url + "/Update"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region Delete
		public static string DeleteName { get { return "controlformargumentdelete"; } }
		public static string Delete { get { return Url + "/Delete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete
	}
}