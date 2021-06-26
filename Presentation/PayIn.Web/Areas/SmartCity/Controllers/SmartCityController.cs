using System.Web.Mvc;

namespace PayIn.Web.Areas.SmartCity.Controllers
{
	public class SmartCityController : Controller
    {
		#region GET SmartCity
		public ActionResult Index()
        {
            return PartialView();
		}
		#endregion GET SmartCity
	}
}