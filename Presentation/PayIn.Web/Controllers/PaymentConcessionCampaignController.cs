using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class PaymentConcessionCampaignController : Controller
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

		#region /ResendNotification
		public ActionResult ResendNotification()
		{
			return PartialView();
		}
		#endregion /ResendNotification

		#region /Delete
		public ActionResult Delete()
		{
			return PartialView();
		}
		#endregion /Delete
	}
}
