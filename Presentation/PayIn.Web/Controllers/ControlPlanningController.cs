using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class ControlPlanningController : Controller
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

		#region /CreateTemplate
		public ActionResult CreateTemplate()
		{
			return PartialView();
		}
		#endregion /CreateTemplate

		#region /Clear
		public ActionResult Clear()
		{
			return PartialView();
		}
		#endregion /Clear

		#region /Delete
		public ActionResult Delete()
		{
			return PartialView();
		}
		#endregion /Delete

		#region /Update
		public ActionResult Update()
		{
			return PartialView();
		}
		#endregion /Update
	}
}