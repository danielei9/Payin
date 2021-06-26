namespace PayIn.Web.App.Factories
{
	public class PaymentUserFactory 
	{
		public static string UrlApi { get { return "/Api/PaymentUser"; } }
		public static string Url { get { return "/PaymentUser"; } }

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get
		
		#region GetAll
		public static string GetAllName { get { return "paymentusergetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Create
		public static string CreateName { get { return "paymentusercreate"; } }
		public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Update
		public static string UpdateName { get { return "paymentuserupdate"; } }
		public static string Update { get { return Url + "/Update"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
		#endregion

		#region Delete
		public static string Delete { get { return "paymentuserdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion

		#region ResendNotification
		public static string ResendNotification { get { return "paymentuserresendnotification"; } }
		public static string ResendNotificationApi { get { return UrlApi + "/Resend"; } }
		#endregion ResendNotification

		#region GetSelector
		public static string GetSelectorApi { get { return UrlApi + "/Selector"; } }
		#endregion GetSelector
	}
}
