using System.Web.Mvc;

namespace PayIn.Web.Areas.SmartCity.Controllers
{
	public class ComponentController : Controller
    {
		#region GET SmartCity/Component
		public ActionResult Index()
        {
            return PartialView();
		}
		#endregion GET SmartCity/Component
	}
}