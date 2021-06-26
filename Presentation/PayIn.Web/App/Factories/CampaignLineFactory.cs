using System;

namespace PayIn.Web.App.Factories
{
	public class CampaignLineFactory
	{
		public static string UrlApi { get { return "/Api/CampaignLine"; } }
		public static string Url { get { return "/CampaignLine"; } }

		#region Create
		public static string CreateName { get { return "campaignlinecreate"; } }
		public static string Create(string campaignId = "") { return CreateName + (campaignId.IsNullOrEmpty() ? "" : "({id:" + campaignId + "})"); }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region CreateImage
		public static string CreateImageName { get { return "campaignlinecreateimage"; } }
		public static string CreateImageApi { get { return UrlApi + "/CreateImage/:id"; } }
		#endregion CreateImage

		#region Get
		public static string GetApi { get { return UrlApi + "/View"; } }
		public static string GetApiId { get { return UrlApi + "/:id"; } }

		#endregion Get

		#region GetAll
		public static string GetAllName { get { return "campaignlinegetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
        #endregion GetAll


        #region Update
        public static string UpdateName { get { return "campaignlineupdate"; } }
		public static string Update { get { return Url + "/Update"; } }
		public static string UpdateApi { get { return UrlApi + "/:id"; } }
		#endregion Update

		#region UpdateImageCrop
		public static string UpdateImageCropName { get { return "campaignlineimagecrop"; } }
		public static string UpdateImageCrop { get { return UrlApi + "/ImageCrop"; } }
		#endregion UpdateImageCrop

		#region Delete
		public static string Delete { get { return "campaignlinedelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete

		#region RetrieveSelector
		public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
		public static string ProductRetrieveSelectorApi { get { return UrlApi + "/ProductRetrieveSelector"; } }
		public static string ProductFamilyRetrieveSelectorApi { get { return UrlApi + "/ProductFamilyRetrieveSelector"; } }
		public static string ServiceUserRetrieveSelectorApi { get { return UrlApi + "/ServiceUserRetrieveSelector"; } }
		public static string ServiceGroupRetrieveSelectorApi { get { return UrlApi + "/ServiceGroupRetrieveSelector"; } }
		#endregion RetrieveSelector


		#region GetByServiceUser
		public static string GetByServiceUserName { get { return "campaignlinegetbyserviceuser"; } }
		public static string GetByServiceUserAll { get { return Url + "/ServiceUser/:id"; } }
		public static string GetByServiceUserAllApi { get { return UrlApi + "/ServiceUser"; } }
		#endregion GetByServiceUser

		#region GetByServiceGroup
		public static string GetByServiceGroupName { get { return "campaignlinegetbyservicegroup"; } }
		public static string GetByServiceGroupAll { get { return Url + "/ServiceGroup/:id"; } }
		public static string GetByServiceGroupAllApi { get { return UrlApi + "/ServiceGroup"; } }
		#endregion GetByServiceGroup

		#region GetByProduct
		public static string GetByProductName { get { return "campaignlinegetbyproduct"; } }
		public static string GetByProductAll { get { return Url + "/Product/:id"; } }
		public static string GetByProductAllApi { get { return UrlApi + "/Product"; } }
		#endregion GetByProduct

		#region GetByProductFamily
		public static string GetByProductFamilyName { get { return "campaignlinegetbyproductfamily"; } }
		public static string GetByProductFamilyAll { get { return Url + "/ProductFamily/:id"; } }
		public static string GetByProductFamilyAllApi { get { return UrlApi + "/ProductFamily"; } }
		#endregion GetByProductFamily

		#region GetByEntranceType
		public static string GetByEntranceTypeName { get { return "campaignlinegetbyentrancetype"; } }
		public static string GetByEntranceTypeAll { get { return Url + "/EntranceType/:id"; } }
		public static string GetByEntranceTypeAllApi { get { return UrlApi + "/EntranceType"; } }
		#endregion GetByEntranceType

		#region AddProduct
		public static string AddProductName { get { return "campaignlineaddproduct"; } }
		public static string AddProduct(string id = "") { return AddProductName + ("({id:\"" + id + "\"})"); }
		public static string AddProductApi { get { return UrlApi + "/Product"; } }
		#endregion AddProduct

		#region AddProductFamily
		public static string AddProductFamilyName { get { return "campaignlineaddproductfamily"; } }
		public static string AddProductFamily(string id = "") { return AddProductFamilyName + ("({id:\"" + id + "\"})"); }
		public static string AddProductFamilyApi { get { return UrlApi + "/ProductFamily"; } }
		#endregion AddProductFamily

		#region AddServiceUser
		public static string AddServiceUserName { get { return "campaignlineaddserviceuser"; } }
		public static string AddServiceUser(string id = "") { return AddServiceUserName + ("({id:\"" + id + "\"})"); }
		public static string AddServiceUserApi { get { return UrlApi + "/ServiceUser"; } }
		#endregion AddServiceUser

		#region AddServiceGroup
		public static string AddServiceGroupName { get { return "campaignlineaddservicegroup"; } }
		public static string AddServiceGroup(string id = "") { return AddServiceGroupName + ("({id:\"" + id + "\"})"); }
		public static string AddServiceGroupApi { get { return UrlApi + "/ServiceGroup"; } }
		#endregion AddServiceGroup

		#region AddEntranceType
		public static string AddEntranceTypeName { get { return "campaignlineaddentrancetype"; } }
		public static string AddEntranceType(string id = "") { return AddEntranceTypeName + ("({id:\"" + id + "\"})"); }
		public static string AddEntranceTypeApi { get { return UrlApi + "/EntranceType"; } }
		#endregion AddEntranceType

		#region RemoveProduct
		public static string RemoveProduct { get { return "campaignlineremoveproduct"; } }
		public static string RemoveProductApi { get { return UrlApi + "/Product/:id"; } }
		#endregion RemoveProduct

		#region RemoveProductFamily
		public static string RemoveProductFamily { get { return "campaignlineremoveproductfamily"; } }
		public static string RemoveProductFamilyApi { get { return UrlApi + "/ProductFamily/:id"; } }
		#endregion RemoveProductFamily

		#region RemoveServiceUser
		public static string RemoveServiceUser { get { return "campaignlineremoveserviceuser"; } }
		public static string RemoveServiceUserApi{ get { return UrlApi + "/RemoveServiceUser/:id"; } }
		#endregion RemoveServiceUser

		#region removeServiceGroup
		public static string RemoveServiceGroup { get { return "campaignlineremoveservicegroup"; } }
		public static string RemoveServiceGroupApi { get { return UrlApi + "/ServiceGroup/:id"; } }
		#endregion removeServiceGroup

		#region RemoveEntranceType
		public static string RemoveEntranceType { get { return "campaignlineremoveentrancetype"; } }
		public static string RemoveEntranceTypeApi { get { return UrlApi + "/EntranceType/:id"; } }
		#endregion RemoveEntranceType
	}
}