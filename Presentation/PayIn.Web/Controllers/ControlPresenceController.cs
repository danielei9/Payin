using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class ControlPresenceController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /

		#region /Graph
		public ActionResult Graph()
		{
			return PartialView();
		}
		#endregion /Graph

		#region /Legacy
		public ActionResult Legacy()
		{
			return PartialView();
		}
		#endregion /Legacy

		#region /Create
		public ActionResult Create()
		{
			return PartialView();
		}
		#endregion /Create

		#region /Update
		public ActionResult Update()
		{
			return PartialView();
		}
		#endregion /Update

		#region /Delete
		public ActionResult Delete()
		{
			return PartialView();
		}
		#endregion /Delete
	}
}