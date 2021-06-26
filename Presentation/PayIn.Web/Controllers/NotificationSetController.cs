using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class NotificationSetController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /

		#region /Create
		public ActionResult Create()
		{
			return PartialView();
		}
		#endregion /Create
	}
}