using System.Web.Mvc;

namespace PayIn.Web.Areas.SmartCity.Controllers
{
	public class EnergyTariffController : Controller
    {
		#region GET SmartCity/EnergyTariff
		public ActionResult Index()
        {
            return PartialView();
		}
		#endregion GET SmartCity/EnergyTariff
	}
}