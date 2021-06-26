namespace PayIn.Web.App.Factories
{
	public class ServiceUserFactory
	{
		public static string UrlApi { get { return "/Api/ServiceUser"; } }
		public static string Url { get { return "/ServiceUser"; } }

		#region Get
		public static string GetName { get { return "serviceuserget"; } }
		public static string Get{ get { return Url; } }
		public static string GetApi { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "serviceusergetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region GetAllByDashBoard
		public static string GetAllByDashboardName { get { return "serviceusergetallbydashboard"; } }
		public static string GetAllByDashboard { get { return Url + "/Dashboard"; } }
		public static string GetAllByDashboardApi { get { return UrlApi + "/Dashboard"; } }
		#endregion GetAllByDashBoard

		#region Create
		public static string CreateName { get { return "serviceusercreate"; } }
		public static string Create(string systemCardId = "") { return CreateName + ("({systemCardId:\"" + systemCardId + "\"})"); }
		public static string CreateApi { get { return UrlApi + "/Create"; } }

		public static string CreateCsv { get { return CreateApi + "/csv"; } }
		#endregion Create

		#region CreateGet
		public static string CreateGet { get { return UrlApi + "/Create"; } }
		#endregion CreateGet

		#region ServiceGroups
		public static string ServiceGroupsName { get { return "serviceuserservicegroups"; } }
		public static string ServiceGroupsApi { get { return UrlApi + "/ServiceGroups"; } }
		public static string ServiceGroups { get { return Url + "/ServiceGroups/{id}"; } }
		#endregion ServiceGroups

		#region AddServiceGroup
		public static string AddServiceGroupName { get { return "serviceuseraddservicegroup"; } }
		public static string AddServiceGroupApi { get { return UrlApi + "/AddServiceGroup"; } }
		public static string AddServiceGroup(string userId) {return AddServiceGroupName + "({id:\"" + userId + "\"})"; }
		//public static string AddServiceGroup { get { return Url + "/AddServiceGroup"; } }
		#endregion AddServiceGroup

		#region RemoveServiceGroup
		public static string RemoveServiceGroupName { get { return "serviceuserremoveservicegroup"; } }
		public static string RemoveServiceGroupApi { get { return UrlApi + "/RemoveServiceGroup"; } }
		#endregion RemoveServiceGroup

		#region CreateCardSelect
		public static string CreateCardSelectName { get { return "serviceusercreatecardselect"; } }
		#endregion CreateCardSelect

		#region CreateCardSelectGet
		public static string CreateCardSelectGetApi { get { return UrlApi + "/CreateCardSelect"; } }
		#endregion CreateCardSelectGet

		#region CreateCardGet
		public static string CreateCardGetApi { get { return UrlApi + "/CreateCard"; } }
		public static string CreateCardGetApiTemplate { get { return UrlApi + "/CreateCard/:id"; } }
		#endregion CreateCardGet

		#region Delete
		public static string Delete { get { return "serviceuserdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion

		#region Register
		public static string RegisterName { get { return "serviceuserregister"; } }
		public static string RegisterApi { get { return UrlApi + "/register"; } }
		#endregion Register

		#region Subscribe
		public static string SubscribeName { get { return "serviceusersubscribe"; } }
		public static string SubscribeApi { get { return UrlApi + "/subscribe"; } }
		#endregion Subscribe

		#region Unsubscribe
		public static string UnsubscribeName { get { return "serviceuserunsubscribe"; } }
		public static string UnsubscribeApi { get { return UrlApi + "/unsubscribe"; } }
		#endregion Unsubscribe

		#region Update
		public static string UpdateName { get { return "serviceuserupdate"; } }
		public static string UpdateApi { get { return UrlApi + "/Update"; } }
		#endregion Update

		#region Details
		public static string DetailsName { get { return "serviceuserdetails"; } }
		public static string DetailsApi { get { return UrlApi + "/serviceuserdetails"; } }
		#endregion Details

		#region RetrieveSelector
		public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
		#endregion RetrieveSelector

		#region CreateImage
		public static string CreateImageName { get { return "serviceusercreateimage"; } }
		public static string CreateImageApi { get { return UrlApi + "/CreateImage/:id"; } }
		#endregion CreateImage

		#region UpdateImageCrop
		public static string UpdateImageCropName { get { return "serviceuserimagecrop"; } }
		public static string UpdateImageCrop { get { return UrlApi + "/ImageCrop"; } }
		#endregion UpdateImageCrop

		#region UpdateCardSelect
		public static string UpdateCardSelectName { get { return "serviceuserupdatecardselect"; } }
		#endregion UpdateCardSelect

		#region UpdateCard
		public static string UpdateCardName { get { return "serviceuserupdatecard"; } }
		public static string UpdateCardApi { get { return UrlApi + "/UpdateCard"; } }
		public static string UpdateCard { get { return Url + "/UpdateCard"; } }
		#endregion UpdateCard

		#region UpdateCardGet
		public static string UpdateCardGetApi { get { return UrlApi + "/UpdateCard"; } }
		public static string UpdateCardGetApiTemplate { get { return UrlApi + "/UpdateCard/:id"; } }
		#endregion UpdateCardGet

	}
}