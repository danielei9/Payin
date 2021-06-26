using System.Web.Mvc;

namespace PayIn.Web.Areas.Bus.Controllers
{
	public class GraphController : Controller
    {
		#region GET Bus/Graph
		public ActionResult Index()
        {
            return PartialView();
		}
		#endregion GET Bus/Graph
	}
}