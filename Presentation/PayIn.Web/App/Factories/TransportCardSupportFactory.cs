using System;

namespace PayIn.Web.App.Factories
{
	public partial class TransportCardSupportFactory
	{
		public static string UrlApi { get { return "/Api/TransportCardSupport"; } }		
		public static string Url { get { return "/TransportCardSupport"; } }

        #region GetAll

        public static string GetAllName{ get { return "transportcardsupportgetall"; } }
        public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		
		#endregion GetAll

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region Create
		public static string CreateName { get { return "transportcardsupportcreate"; } }
		public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Update
		public static string UpdateName { get { return "transportcardsupportupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update	
	
		#region Delete
		public static string Delete { get { return "transportcardsupportdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion

		#region Selector
		public static string RetrieveSelectorApi { get { return UrlApi + "/Selector"; } }
        #endregion Selector

    }
}
