using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayIn.Web.App.Factories
{
	public class PaymentConcessionPurseFactory
	{
		public static string UrlApi { get { return "/Api/PaymentConcessionPurse"; } }
		public static string Url { get { return "/PaymentConcessionPurse"; } }
				
		#region Get
		public static string GetApi { get { return UrlApi + "/View"; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "paymentconcessionpursegetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll
		
		#region Delete
		public static string Delete { get { return "paymentconcessionpursedelete"; } }
		public static string DeleteApi { get { return UrlApi + "/Delete"; } }
		#endregion Delete

		#region ResendNotification
		public static string ResendNotification { get { return "paymentconcessionpurseresendnotification"; } }
		public static string ResendNotificationApi { get { return UrlApi + "/Resend"; } }
		#endregion ResendNotification

		#region Create
		public static string CreateName { get { return "paymentconcessionpursecreate"; } }
		public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi + "/Create"; } }
		#endregion Create
	}
}