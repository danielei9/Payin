using System.Web.Mvc;

namespace PayIn.Web.Security.Controllers
{
	public class ServiceCardBatchController : Controller
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
        
		#region /Lock
		public ActionResult Lock()
		{
			return PartialView();
		}
		#endregion /Lock

		#region /Unlock
		public ActionResult Unlock()
		{
			return PartialView();
		}
		#endregion /Unlock

		#region /Delete
		[HttpGet]
		public ActionResult Delete()
		{
			return PartialView();
		}
		#endregion /Delete
	}
}