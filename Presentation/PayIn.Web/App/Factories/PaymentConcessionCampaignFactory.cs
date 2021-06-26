namespace PayIn.Web.App.Factories
{
	public class PaymentConcessionCampaignFactory 
	{
		public static string UrlApi { get { return "/Api/PaymentConcessionCampaign"; } }
		public static string Url { get { return "/PaymentConcessionCampaign"; } }

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "paymentconcessioncampaigngetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll		

		#region Create
		public static string CreateName { get { return "paymentconcessioncampaigncreate"; } }
		public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi + "/Create"; } }
		#endregion Create

		#region Update
		public static string UpdateName { get { return "paymentconcessioncampaignupdate"; } }
		public static string Update { get { return Url + "/Update"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
		#endregion Update

		#region Delete
		public static string Delete { get { return "paymentconcessioncampaigndelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete

		#region ResendNotification
		public static string ResendNotification { get { return "paymentconcessioncampaignresendnotification"; } }
		public static string ResendNotificationApi { get { return UrlApi + "/Resend"; } }
		#endregion ResendNotification
	}
}
