using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class ServiceNotificationController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /
	}
}