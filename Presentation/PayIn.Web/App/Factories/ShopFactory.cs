namespace PayIn.Web.App.Factories
{
	public class ShopFactory
	{
		public static string UrlApi { get { return "/Api/Shop"; } }
		public static string Url { get { return "/Shop"; } }

        //#region GetAll
        //public static string GetAllName { get { return "shopgetall"; } }
        //public static string GetAll { get { return Url; } }
        //public static string GetAllApi { get { return UrlApi; } }
        //      #endregion GetAll

        //      #region GetCurrent
        //      public static string GetCurrent { get { return "shopcurrent"; } }
        //      public static string GetCurrentApi { get { return UrlApi + "/Current"; } }
        //      #endregion GetCurrent

        #region GetConcessionEvents
        public static string ConcessionsEvents      { get { return "concessionEvents"; } }
        public static string GetConcessionEvents    { get { return Url +"/Concession"; } }
        public static string GetApiConcessionEvents { get { return UrlApi + "/Concession/:id"; } }
        #endregion GetConcessionEvents

        #region GetEventsEntrancesType
        public static string EventEntrancesType { get { return "evententrancetype"; } }
		public static string GetEventEntrancesType { get { return Url + "/Event"; } }
		public static string GetApiEventEntrancesType { get { return UrlApi + "/Event/:id"; } }
        #endregion GetEventsEntrancesType

        #region GetEntrance
        public static string Entrance { get { return "entrance"; } }
        public static string GetEntrance { get { return Url + "/Entrance"; } }
		public static string GetApiEntrance { get { return UrlApi + "/Entrance/:id"; } }
		#endregion GetEntrance

		#region GetByConcession
		public static string GetByConcession { get { return Url + "/ByConcession"; } }
		public static string GetApiByConcession { get { return UrlApi + "/ByConcession"; } }
		#endregion GetByConcession

		#region GetForm
		public static string FormEntrance { get { return "entranceform"; } }
        public static string GetFormEntrance { get { return Url + "/Entrance/Form"; } }
        public static string GetApiFormEntrance { get { return UrlApi + "/Entrance/:id/Form"; } }
        #endregion GetForm

        #region RetrieveSelector
        public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
		#endregion RetrieveSelector

		#region RetrieveMyCardsSelector
		public static string RetrieveMyCardsSelectorApi { get { return UrlApi + "/RetrieveMyCardsSelector"; } }
		#endregion RetrieveMyCardsSelector	}	}
	}
}
