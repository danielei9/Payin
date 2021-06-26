using System;

namespace PayIn.Web.App.Factories
{
	public partial class ExhibitorFactory
	{
		public static string UrlApi { get { return "/Api/Exhibitor"; } }
		public static string Url { get { return "/Exhibitor"; } }

        #region GetAll
        public static string GetAllName { get { return "exhibitorgetall"; } }
        public static string GetAll { get { return Url; } }
        public static string GetAllApi { get { return UrlApi; } }
        #endregion GetAll

        #region Delete
        public static string Delete { get { return "exhibitordelete"; } }
        public static string DeleteApi { get { return UrlApi + "/:id"; } }
        #endregion Delete

        #region Update
        public static string UpdateName { get { return "exhibitorupdate"; } }
        public static string UpdateApi { get { return UrlApi + "/:id"; } }
        #endregion Update

        #region Suspend
        public static string SuspendName { get { return "exhibitorsuspend"; } }
        public static string SuspendApi { get { return UrlApi + "/Suspend/:id"; } }
        #endregion Suspend

        #region Unsuspend
        public static string UnsuspendName { get { return "exhibitorunsuspend"; } }
        public static string UnsuspendApi { get { return UrlApi + "/Unsuspend/:id"; } }
        #endregion Unsuspend

        #region RetrieveSelector
        public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
        #endregion RetrieveSelector

        #region Create
        public static string CreateName { get { return "exhibitorcreate"; } }
        public static string Create(string eventId = "") { return CreateName + (eventId.IsNullOrEmpty() ? "" : "({id:\"" + eventId + "\"})"); }
        public static string CreateApi { get { return UrlApi; } }
        #endregion Create
    }
}