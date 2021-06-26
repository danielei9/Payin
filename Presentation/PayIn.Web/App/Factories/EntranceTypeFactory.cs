using System;

namespace PayIn.Web.App.Factories
{
	public class EntranceTypeFactory
	{
		public static string UrlApi { get { return "/Api/EntranceType"; } }
		public static string Url { get { return "/EntranceType"; } }

		#region Get
		public static string GetApi { get { return UrlApi; } }
		public static string GetApiTemplate { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "entrancetypegetall"; } }
		public static string GetAll { get { return Url + "/:id"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Create
		public static string CreateName { get { return "entrancetypecreate"; } }
		public static string Create(string eventId = "") { return CreateName + (eventId.IsNullOrEmpty() ? "" : "({id:\"" + eventId + "\"})"); }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region GetVisibility
		public static string GetVisibility { get { return UrlApi; } }
		public static string GetVisibilityApi { get { return UrlApi + "/:id"; } }
		#endregion GetVisibility

		#region Visibility
		public static string VisibilityName { get { return "entrancetypevisibility"; } }
		public static string VisibilityApi { get { return UrlApi + "/:id"; } }
		#endregion Visibility

		#region Delete
		public static string Delete { get { return "entrancetypedelete"; } }
		public static string DeleteApi { get { return UrlApi + "/:id"; } }
		#endregion Delete

		#region Relocate
		public static string RelocateName { get { return "entrancetyperelocate"; } }
		public static string Relocate { get { return Url + "/Relocate"; } }
		public static string RelocateApi { get { return UrlApi + "/Relocate/:id"; } }
		#endregion Relocate

		#region RetrieveSelector
		public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
		#endregion RetrieveSelector

		#region Update
		public static string UpdateName { get { return "entrancetypeupdate"; } }
		public static string Update { get { return Url + "/Update"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
		#endregion Update

		#region CreateImage
		public static string CreateImageName { get { return "entrancetypecreateimage"; } }
		public static string CreateImageApi { get { return UrlApi + "/CreateImage/:id"; } }
		#endregion CreateImage

		#region UpdateImageCrop
		public static string UpdateImageCropName { get { return "entrancetypeimagecrop"; } }
		public static string UpdateImageCrop { get { return UrlApi + "/ImageCrop"; } }
		#endregion UpdateImageCrop

		#region IsVisible
		public static string IsVisibleApi { get { return UrlApi + "/IsVisible/:id"; } }
		#endregion IsVisible

		#region Show
		public static string Show { get { return "entrancetypeshow"; } }
		#endregion Show

		#region Hide
		public static string Hide { get { return "entrancetypehide"; } }
		#endregion Hide

		#region Groups
		public static string GetAllGroupsName { get { return "entrancetypegroupsgetall"; } }
		public static string GetAllGroups { get { return Url + "/Groups/:id"; } }
		public static string GetAllGroupsApi { get { return UrlApi + "/Groups/:id"; } }
		#endregion Groups

		#region AddGroup
		public static string AddGroupName { get { return "entrancetypeaddgroup"; } }
		public static string AddGroup(string id = "") { return AddGroupName + "({id:\"" + id + "\"})"; }
		public static string AddGroupApi { get { return UrlApi + "/AddGroup"; } }
		#endregion AddGroup

		#region RemoveGroup
		public static string RemoveGroup { get { return "entrancetyperemovegroup"; } }
		public static string RemoveGroupApi { get { return UrlApi + "/RemoveGroup/:id"; } }
		#endregion RemoveGroup

		#region GetSellable
		public static string GetSellableName { get { return "entrancetypegetsellable"; } }
		public static string GetSellable { get { return Url + "/GetSellable"; } }
		public static string GetSellableApi { get { return UrlApi + "/GetSellable"; } }
        #endregion GetSellable

        #region GetBuyable
        public static string GetBuyableName { get { return "entrancetypegetbuyable"; } }
        public static string GetBuyable { get { return Url + "/GetBuyable"; } }
        public static string GetBuyableApi { get { return UrlApi + "/GetBuyable"; } }
        #endregion GetBuyable

        #region GetEntranceTypesToGive
        public static string GetToGiveName { get { return "entrancetypegettogive"; } }
		public static string GetToGive { get { return Url + "/GetToGive"; } }
		public static string GetToGiveApi { get { return UrlApi + "/GetToGive"; } }
		#endregion GetEntranceTypesToGive

		#region Recharge
		public static string RechargeName { get { return "entrancetyperecharge"; } }
		public static string Recharge { get { return Url + "/Recharge"; } }
		public static string RechargeApi { get { return UrlApi + "/GetEntranceTypeRecharge"; } }
		#endregion Recharge		

		#region Donate
		public static string DonateName { get { return "entrancetypedonate"; } }
		public static string Donate { get { return Url + "/Donate"; } }
		public static string DonateApi { get { return UrlApi + "/GetEntranceTypeDonate"; } }
		#endregion Donate		

		#region BuyRecharge
		public static string BuyRechargeName { get { return "entrancetypebuyrecharge"; } }
		public static string BuyRecharge { get { return Url + "/BuyRecharge"; } }
		public static string BuyRechargeApi { get { return UrlApi + "/GetEntranceTypeBuyBalance"; } }
		#endregion BuyRecharge		
	}
}