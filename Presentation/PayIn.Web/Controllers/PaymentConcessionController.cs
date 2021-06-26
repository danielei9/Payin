using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class PaymentConcessionController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /

		#region /Create
		public ActionResult Create()
		{
			return PartialView();
		}
		#endregion /Create

		#region /CreateImage
		public ActionResult CreateImage()
		{
			return PartialView();
		}
		#endregion /CreateImage

		#region /ImageCrop
		public ActionResult ImageCrop()
		{
			return PartialView();
		}
		#endregion /ImageCrop

		#region /BannerImageCrop
		public ActionResult BannerImageCrop()
		{
			return PartialView();
		}
		#endregion /BannerImageCrop

		#region /CreateCommerce
		public ActionResult CreateCommerce()
		{
			return PartialView();
		}
		#endregion /CreateCommerce

		#region /Update
		public ActionResult Update()
		{
			return PartialView();
		}
		#endregion /Update

		#region /UpdateCommerce
		public ActionResult UpdateCommerce()
		{
			return PartialView();
		}
		#endregion /UpdateCommerce

		#region /Afilliate
		public ActionResult Afilliate()
		{
			return PartialView();
		}
		#endregion/Afilliate

	}
}