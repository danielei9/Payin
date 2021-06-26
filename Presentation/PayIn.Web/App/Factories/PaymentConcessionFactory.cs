using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayIn.Web.App.Factories
{
	public partial class PaymentConcessionFactory
	{
		public static string UrlApi { get { return "/Api/PaymentConcession"; } }
		public static string Url { get { return "/PaymentConcession"; } }

        #region GetAll
        public static string GetAllName { get { return "paymentconcessiongetall"; } }
        public static string GetAllApi { get { return UrlApi; } }
        #endregion GetAll

        #region Get
        public static string GetSupplierApi { get { return "/Api/ServiceSupplier/Current"; } }
		#endregion Get

		#region Create
		public static string CreatePaymentConcessionName { get { return "paymentconcessioncreate"; } }
		public static string CreatePaymentConcessionApi { get { return UrlApi + "/Create"; } }
		public static string CreatePaymentConcessionCommerceName { get { return "paymentconcessioncommercecreate";} }
		#endregion Create

		#region Update
		public static string UpdateName { get { return "paymentconcessionupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update	

		#region CreateImage
		public static string CreateImageName { get { return "paymentconcessioncreateimage"; } }
		public static string CreateImageApi { get { return UrlApi + "/CreateImage/:id"; } }
		#endregion CreateImage

		#region UpdateImageCrop
		public static string UpdateImageCropName { get { return "paymentconcessionimagecrop"; } }
		public static string UpdateImageCrop { get { return UrlApi + "/ImageCrop"; } }
		#endregion UpdateImageCrop

		#region UpdateBannerImageCrop
		public static string UpdateBannerImageCropName { get { return "paymentconcessionbannerimagecrop"; } }
		public static string UpdateBannerImageCrop { get { return UrlApi + "/BannerImageCrop"; } }
		#endregion UpdateBannerImageCrop

		#region UpdateCommerce
		public static string UpdateCommerce { get { return "paymentconcessionupdatecommerce"; } }
		public static string UpdateCommerceApi { get { return UrlApi + "/UpdateCommerce"; } }
		#endregion UpdateCommerce

		#region GetCommerce
		public static string GetCommerceApi { get { return UrlApi + "/GetCommerce"; } }
		#endregion GetCommerce

		#region GetSelector
		public static string GetSelectorApi { get { return UrlApi + "/Selector"; } }
		#endregion GetSelector

		#region Selector
		public static string RetrieveSelectorConcessionApi { get { return UrlApi + "/SelectorConcession"; } }
		#endregion Selector

		#region Afilliate
		public static string CreatePaymentConcessionAfilliateName { get { return "paymentconcessionafilliatecreate"; } }
		public static string CreatePaymentConcessionAfilliateApi { get { return UrlApi + "/Afilliate"; } }
		#endregion

	}
}