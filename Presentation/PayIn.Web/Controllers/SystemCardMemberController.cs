using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class SystemCardMemberController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /

		#region /Update
		public ActionResult Update()
		{
			return PartialView();
		}
		#endregion /Update

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
		
	}
}