using System.Web.Mvc;

namespace PayIn.Web.Areas.SmartCity.Controllers
{
	public class DeviceController : Controller
    {
		#region GET SmartCity/Device
		public ActionResult Index()
        {
            return PartialView();
		}
		#endregion GET SmartCity/Device
	}
}