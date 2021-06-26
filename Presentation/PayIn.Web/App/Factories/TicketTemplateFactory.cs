using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayIn.Web.App.Factories
{
	public partial class TicketTemplateFactory
	{
		public static string UrlApi { get { return "Api/TicketTemplate"; } }
		public static string Url { get { return "/TicketTemplate"; } }

		#region Create
		public static string CreateName { get { return "tickettemplatecreate"; } }
		public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region GetCreate
		public static string GetCreateApi { get { return UrlApi; } }
		#endregion GetCreate

		#region Update
		public static string UpdateName { get { return "tickettemplateupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update

		#region Check
		public static string CheckApi { get {return UrlApi + "/Check"; } }
		#endregion Check

		#region Selector
		public static string SelectorApi { get { return UrlApi + "/Selector"; } }
		#endregion Selector

		#region GetAll
		public static string GetAllName { get { return "tickettemplategetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }		
		#endregion GetAll

		#region Details
		public static string DetailsName { get { return "tickettemplatedetails"; } }
		public static string DetailsApi { get { return UrlApi + "/Details"; } }
		#endregion Details

		#region Delete
		public static string Delete { get { return "tickettemplatedelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete
	}
}
