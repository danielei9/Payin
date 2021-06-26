using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayIn.Web.App.Factories
{
	public class PaymentWorkerFactory
	{
		public static string UrlApi { get { return "/Api/PaymentWorker"; } }
		public static string Url { get { return "/PaymentWorker"; } }

		#region GetAll
		public static string GetAll { get { return "paymentworkergetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region GetAllConcession
		public static string GetAllConcession { get { return "paymentworkergetallconcession"; } }
		public static string GetAllConcessionApi { get { return UrlApi + "/Concession"; } }
		#endregion GetAllConcession

		#region Create
		public static string Create { get { return "paymentworkercreate"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region InviteUser
		public static string InviteUser { get { return "paymentworkerinviteuser"; } }
		public static string InviteUserApi { get { return UrlApi + "/InviteUser"; } }
		#endregion InviteUser

		#region InvitedUsers
		public static string InvitedUsers { get { return "paymentworkerinvitedusersgetall"; } }
		public static string InvitedUsersApi { get { return UrlApi + "/InvitedUsers"; } }
		#endregion InvitedUsers

		#region Delete
		public static string Delete { get { return "paymentworkerdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete

		#region DissociateConcession
		public static string DissociateConcession { get { return "paymentworkerdissociateconcession"; } }
		public static string DissociateConcessionApi { get { return UrlApi + "/DissociateConcession"; } }
		#endregion DissociateConcession

		#region ResendNotification
		public static string ResendNotification { get { return "paymentworkerresendnotification"; } }
		public static string ResendNotificationApi { get { return UrlApi; } }
		#endregion ResendNotification
	}
}