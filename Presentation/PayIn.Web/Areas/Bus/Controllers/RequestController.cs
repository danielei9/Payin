using System.Web.Mvc;

namespace PayIn.Web.Areas.Bus.Controllers
{
	public class RequestController : Controller
    {
		#region GET Bus/Request/Create
		public ActionResult Create()
		{
			return PartialView();
		}
		#endregion GET Bus/Request/Create

		#region GET Bus/Request/Delete
		public ActionResult Delete()
		{
			return PartialView();
		}
		#endregion GET Bus/Request/RemovDeleteeRequest
	}
}