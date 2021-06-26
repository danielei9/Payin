using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class EntranceFormValueController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /
	}
}