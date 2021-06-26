using System;

namespace PayIn.Web.App.Factories
{
	public class ShipmentFactory
	{
		public static string UrlApi { get { return "/Api/Shipment"; } }
		public static string Url { get { return "/Shipment"; } }

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "shipmentgetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Create
		public static string CreateName { get { return "shipmentcreate"; } }
		public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi; } }	
		#endregion Create
	

		#region GetAddUsers
		public static string GetAddUserApi { get { return UrlApi + "/AddUser/:id"; } }
		#endregion GetAddUsers

		#region AddUsers
		public static string AddUsersName { get { return "shipmentaddusers"; } }
		public static string AddUsers(string shipmentId = "") { return AddUsersName + (shipmentId.IsNullOrEmpty() ? "" : "({id:" + shipmentId + "})"); }
		public static string AddUsersApi { get { return UrlApi + "/Tickets"; } }
		#endregion AddUsers

		#region Update
		public static string UpdateName { get { return "shipmentupdate"; } }
		public static string Update { get { return Url + "/Update"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
		#endregion

		#region Delete
		public static string Delete { get { return "shipmentdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion

		#region TicketDelete
		public static string DeleteTicket { get { return "shipmentticketdelete"; } }
		public static string DeleteTicketApi { get { return UrlApi+"/Ticket"; } }
		#endregion

		#region Detail
		public static string DetailName { get { return "shipmentdetails"; } }
		public static string Detail { get { return Url + "/Details"; } }
		public static string DetailApi { get { return UrlApi + "/Details"; } }		
		#endregion Detail

		#region Receipts
		public static string ReceiptName { get { return "shipmentreceiptgetall"; } }
		public static string Receipt { get { return Url + "/Receipt"; } }
		public static string ReceiptApi { get { return UrlApi + "/Receipt"; } }

		#endregion Receipts

	}
}
