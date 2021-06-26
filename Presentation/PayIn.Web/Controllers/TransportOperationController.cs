using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class TransportOperationController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /

		#region /Control
		public ActionResult Control()
		{
			return PartialView();
		}
		#endregion /Control
	}
}
