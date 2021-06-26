namespace PayIn.Web.App.Factories
{
	public partial class ControlTrackFactory
	{
		public static string UrlApi { get { return "/Api/ControlTrack"; } }
		public static string Url { get { return "/ControlTrack"; } }

		#region Get
		public static string GetName { get { return "controltrackget"; } }
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "controltrackgetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region GetItem
		public static string GetItemName { get { return "controltrackgetitem"; } }
		public static string GetItemApi { get { return UrlApi + "/Item"; } }
		#endregion GetItem

		#region GetView
		public static string GetViewName { get { return "controltrackgetview"; } }
		public static string GetViewApi { get { return UrlApi; } }
		#endregion GetView
	}
}