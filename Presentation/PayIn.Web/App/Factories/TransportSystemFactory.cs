namespace PayIn.Web.App.Factories
{
	public partial class TransportSystemFactory
	{
		public static string UrlApi { get { return "/Api/TransportSystem"; } }		
		public static string Url { get { return "/TransportSystem"; } }		

		#region GetAll
		public static string GetAllName { get { return "transportsystemgetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }		
		#endregion GetAll

		
		#region Create
		public static string CreateName { get { return "transportsystemcreate"; } }
		public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Update
		public static string UpdateName { get { return "tranpsportsystemupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update	
	
		#region Delete
		public static string Delete { get { return "transportsystemdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion


	}
}
