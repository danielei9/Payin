using System;

namespace PayIn.Web.App.Factories
{
	public partial class TransportSimultaneousTitleCompatibilitiesFactory
    {
        public static string UrlApi { get { return "/Api/TransportSimultaneousTitleCompatibilities"; } }
        public static string Url { get { return "/TransportSimultaneousTitleCompatibilities"; } }

        #region GetAll
        public static string GetAllName { get { return "transportsimultaneoustitlecompatibilitiesgetall"; } }
        public static string GetAll(string titleId = "") { return GetAllName + (titleId.IsNullOrEmpty() ? "" : "({titleId:" + titleId + "})"); }
        public static string GetAllApi { get { return UrlApi ; } }
        #endregion GetAll

        #region Get
        public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region Create
		public static string CreateName { get { return "transportsimultaneoustitlecompatibilitiescreate"; } }
        public static string Create(string titleId = "") { return CreateName + (titleId.IsNullOrEmpty() ? "" : "({titleId:" + titleId + "})"); }
        //public static string Create { get { return Url + "/Create"; } }
        public static string CreateApi { get { return UrlApi; } }
        public static string CreateGetNameApi { get { return UrlApi + "/CreateGetName"; } }
        #endregion Create

        #region Update
        public static string UpdateName { get { return "transportsimultaneoustitlecompatibilitiesupdate"; } }
        public static string Update(string titleId = "") { return UpdateName + (titleId.IsNullOrEmpty() ? "" : "({titleId:" + titleId + "})"); }
        public static string UpdateApi { get { return UrlApi; } }
		public static string UpdateApiId { get { return UrlApi + "/update"; } }
		#endregion Update	

		#region Delete
		public static string Delete { get { return "transportsimultaneoustitlecompatibilitiesdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion

		#region Selector
		public static string RetrieveSelectorApi { get { return UrlApi + "/Selector"; } }
        #endregion Selector

       

    }
}
