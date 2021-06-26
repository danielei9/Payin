using System.Web.Mvc;

namespace PayIn.Web.JustMoney.Controllers
{
	public class HomeController : Controller
	{
		#region /
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}
		#endregion /
	}
}