using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class BlackListController : Controller
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

		#region /Delete
		public ActionResult Delete()
		{
			return PartialView();
		}
		#endregion /Delete
	}
}
