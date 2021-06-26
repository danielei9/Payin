namespace PayIn.Web.App.Factories.Bus
{
	public class LineFactory
	{
		public static string UrlApi { get { return "/Bus/Api/Line"; } }
		public static string Url { get { return "/Bus/Line"; } }
		
		#region GetAll
		public static string GetAllName { get { return "buslinegetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
        #endregion GetAll

        #region GetStops
        public static string GetStopsName { get { return "buslinegetstops"; } }
        public static string GetStops { get { return Url + "/Stops"; } }
        public static string GetStopsApi { get { return UrlApi + "/Stops"; } }
		#endregion GetStops

		#region GetItinerary
		public static string GetItineraryName { get { return "buslinegetitinerary"; } }
		public static string GetItinerary { get { return Url + "/Itinerary"; } }
		public static string GetItineraryApi { get { return UrlApi + "/Itinerary"; } }
		#endregion GetItinerary

		#region Selector
		public static string SelectorApi { get { return UrlApi + "/Selector"; } }
		#endregion Selector
	}
}