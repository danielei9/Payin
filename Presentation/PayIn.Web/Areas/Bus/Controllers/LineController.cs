using System.Web.Mvc;

namespace PayIn.Web.Areas.Bus.Controllers
{
	public class LineController : Controller
    {
		#region GET Bus/Line
		public ActionResult Index()
        {
            return PartialView();
		}
        #endregion GET Bus/Line

		#region GET Bus/Line/Itinerary
		public ActionResult Itinerary()
		{
			return PartialView();
		}
		#endregion GET Bus/Line/Itinerary
	}
}