using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class ServiceConcessionController : Controller
	{
		#region /Index
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /Index

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

		#region /UpdateState
		public ActionResult UpdateState()
		{
			return PartialView();
		}
		#endregion /UpdateState

		#region /Delete
		public ActionResult Delete()
		{
			return PartialView();
		}
		#endregion /Delete
	}
}