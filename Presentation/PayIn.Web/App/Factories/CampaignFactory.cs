using System;

namespace PayIn.Web.App.Factories
{
	public class CampaignFactory 
	{
		public static string UrlApi { get { return "/Api/Campaign"; } }
		public static string Url { get { return "/Campaign"; } }

		#region Get
		public static string GetApi { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "campaigngetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region GetAddOwners
		public static string GetAddOwnersApi { get { return UrlApi + "/AddOwners"; } }
		#endregion GetAddOwners

		#region Create
		public static string CreateName { get { return "campaigncreate"; } }
		public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi; } }
		public static string CreateGetApi { get { return UrlApi + "/Create"; } }
		#endregion Create

		#region CreateImage
		public static string CreateImageName { get { return "campaigncreateimage"; } }
		public static string CreateImageApi { get { return UrlApi + "/CreateImage/:id"; } }
		#endregion CreateImage

		#region Update
		public static string UpdateName { get { return "campaignupdate"; } }
		public static string Update { get { return Url + "/Update"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
		#endregion Update

		#region UpdateImageCrop
		public static string UpdateImageCropName { get { return "campaignimagecrop"; } }
		public static string UpdateImageCrop { get { return UrlApi + "/ImageCrop"; } }
		#endregion UpdateImageCrop

		#region Suspend
		public static string SuspendName { get { return "campaignsuspend"; } }
		public static string SuspendApi { get { return UrlApi + "/Suspend/:id"; } }
		#endregion Suspend

		#region Unsuspend
		public static string UnsuspendName { get { return "campaignunsuspend"; } }
		public static string UnsuspendApi { get { return UrlApi + "/Unsuspend/:id"; } }
		#endregion Unsuspend

		#region Delete
		public static string Delete { get { return "campaigndelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion

		#region GetSelector
		public static string GetSelectorApi { get { return UrlApi + "/Selector"; } }
        #endregion GetSelector	

        #region GetEvent
        public static string GetEventName { get { return "campaigngetevent"; } }
        public static string GetEvent { get { return Url; } }
        public static string GetEventApi { get { return UrlApi +"/Event"; } }
        #endregion GetEvent

        #region AddEvent
        public static string AddEventName { get { return "campaignaddevent"; } }
        public static string AddEvent(string id = "") { return AddEventName + ("({id:\"" + id + "\"})"); }
        public static string AddEventApi { get { return UrlApi + "/AddEvent"; } }
        #endregion AddEvent

        #region RemoveEvent
        public static string RemoveEvent { get { return "campaignremoveevent"; } }
        public static string RemoveEventApi { get { return UrlApi + "/RemoveEvent/:id"; } }
        #endregion RemoveEvent

        #region retrieveSelector
        public static string EventRetrieveSelectorApi { get { return UrlApi + "/EventRetrieveSelector"; } }
        #endregion retrieveSelector
    }
}
