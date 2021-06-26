using System.Web.Mvc;

namespace PayIn.Web.Areas.JustMoney.Controllers
{
	public class HomeController : Controller
    {
		#region GET JustMoney
		public ActionResult Index()
        {
            return View();
		}
		#endregion GET JustMoney
	}
}