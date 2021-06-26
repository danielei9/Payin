using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class PromotionController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /

		#region /GetAllConcession
		public ActionResult GetAllConcession()
		{
			return PartialView();
		}
		#endregion /GetAllConcession

		#region /Create
		public ActionResult Create()
		{
			return PartialView();
		}
		#endregion /Create

		#region /Delete
		public ActionResult Delete()
		{
			return PartialView();
		}
		#endregion /Delete

		#region /UnlinkCode
		public ActionResult UnlinkCode()
		{
			return PartialView();
		}
		#endregion /UnlinkCode

		#region /ViewCode
		public ActionResult ViewCode()
		{
			return PartialView();
		}
		#endregion /ViewCode

		#region /AddCode
		public ActionResult AddCode()
		{
			return PartialView();
		}
		#endregion /AddCode

		#region /Update
		public ActionResult Update()
		{
			return PartialView();
		}
		#endregion /Update
	}
}
