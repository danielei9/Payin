using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class AccessControlEntranceController : Controller
	{
		#region /
		
		public ActionResult Index()
		{
			return PartialView();
		}

		#endregion

		#region /Create

		public ActionResult Create()
		{
			return PartialView();
		}

		#endregion

		#region /Update

		public ActionResult Update()
		{
			return PartialView();
		}

		#endregion

		#region /Delete

		public ActionResult Delete()
		{
			return PartialView();
		}

		#endregion
	}
}