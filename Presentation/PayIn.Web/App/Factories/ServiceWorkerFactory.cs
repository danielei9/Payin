using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayIn.Web.App.Factories
{
	public partial class ServiceWorkerFactory
	{
		public static string UrlApi { get { return "/Api/ServiceWorker"; } }
		public static string Url { get { return "/ServiceWorker"; } }

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "serviceworkergetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region GetControl
		public static string GetControlName { get { return "serviceworkergetcontrol"; } }
		public static string GetControl { get { return Url + "/Control"; } }
		public static string GetControlApi { get { return UrlApi + "/Control"; } }
		#endregion GetControl

		#region Create
		public static string CreateName { get { return "serviceworkercreate"; } }
		public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Update
		public static string WorkerUpdateName { get { return "serviceworkerupdate"; } }
		public static string WorkerUpdate { get { return Url + "/Update"; } }
		public static string WorkerUpdateApi { get { return UrlApi + "/:id"; } }
		#endregion

		#region Delete
		public static string Delete { get { return "serviceworkerdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion

		#region Selector
		public static string RetrieveSelectorApi { get { return UrlApi + "/Selector"; } }
		#endregion Selector

		#region AcceptAssignment
		public static string AcceptAssignmentName { get { return "serviceworkeracceptassignment"; } }
		public static string AcceptAssignment { get { return Url + "/acceptassignment"; } }
		public static string AcceptAssignmentApi { get { return UrlApi + "/AcceptAssignment"; } }
		#endregion AcceptAssignment
	}
}
