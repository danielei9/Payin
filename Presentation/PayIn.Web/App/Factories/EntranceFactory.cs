using System;

namespace PayIn.Web.App.Factories
{
    public class EntranceFactory
    {
        public static string UrlApi { get { return "/Api/Entrance"; } }
        public static string Url { get { return "/Entrance"; } }

        #region Get
        public static string Get { get { return UrlApi; } }
        public static string GetApi { get { return UrlApi + "/:id"; } }
        #endregion Get

        #region GetAll
        public static string GetAllName { get { return "entrancegetall"; } }
        public static string GetAll { get { return Url; } }
        public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Create
		public static string CreateName { get { return "entrancecreate"; } }
		public static string Create { get { return Url + "/Create/:id"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Invite
		public static string InviteName { get { return "entranceinvite"; } }
		public static string Invite(string entranceTypeId = "") { return InviteName + (entranceTypeId.IsNullOrEmpty() ? "" : "({id:" + entranceTypeId + "})"); }
		public static string InviteApi { get { return UrlApi + "/Invite"; } }
        #endregion Invite

        #region Valdation
        public static string Validation { get { return "entrancevalidation"; } }
        public static string ValidationApi { get { return UrlApi + "/Validation/:code"; } }
        #endregion Valdation

        #region Mail
        public static string PDF { get { return "generatePDF"; } }
        public static string PDFApi { get { return UrlApi + "/Mail/:id"; } }
		#endregion Mail

		#region EntranceTicket
		public static string EntranceTicket { get { return "entranceticket"; } }
		public static string EntranceTicketApi { get { return UrlApi + "/EntranceTicket/:id"; } }
		#endregion EntranceTicket

		#region Suspend
		public static string SuspendName { get { return "entrancesuspend"; } }
		public static string SuspendApi { get { return UrlApi + "/Suspend/:id"; } }
		#endregion Suspend

		#region Unsuspend
		public static string UnsuspendName { get { return "entranceunsuspend"; } }
		public static string UnsuspendApi { get { return UrlApi + "/Unsuspend/:id"; } }
		#endregion Unsuspend

		#region ChangeCard
		public static string ChangeCardName { get { return "entrancechangecard"; } }
		public static string ChangeCardApi { get { return UrlApi + "/ChangeCard/:id"; } }
		#endregion ChangeCard
	}
}