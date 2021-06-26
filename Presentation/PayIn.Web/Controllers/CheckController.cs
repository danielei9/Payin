using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class CheckController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /
	}
}