using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class ServiceGroupController : Controller
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

		#region /Update
		public ActionResult Update()
		{
			return PartialView();
		}
		#endregion /Update

		#region /AddUser
		public ActionResult AddUser()
		{
			return PartialView();
		}
		#endregion /AddUser
	}
}
