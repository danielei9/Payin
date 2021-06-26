using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class ControlTrackController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /

		#region /Item
		public ActionResult Item()
		{
			return PartialView();
		}
		#endregion /Item

		#region /View
		public new ActionResult View()
		{
			return PartialView();
		}
		#endregion /View
	}
}