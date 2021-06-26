using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class SystemCardController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /

		#region /Delete
		public ActionResult Delete()
		{
			return PartialView();
		}
		#endregion /Delete

		#region /Create
		public ActionResult Create()
		{
			return PartialView();
		}
		#endregion /Create

		#region /Update
		public ActionResult Update()
		{
			return PartialView();
		}
		#endregion /Update

		#region /ImageCrop
		public ActionResult ImageCrop()
		{
			return PartialView();
		}
		#endregion /ImageCrop

	}
}