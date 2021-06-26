using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class ServiceWorkerController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /

		#region /Control
		public ActionResult Control()
		{
			return PartialView();
		}
		#endregion /Control

		#region /Create
		public ActionResult Create()
		{
			return PartialView();
		}
		#endregion /Create

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

		#region /AcceptAssignment
		public ActionResult AcceptAssignment()
		{
			return PartialView();
		}
		#endregion /AcceptAssignment
	}
}
