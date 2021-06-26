using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class UserController : Controller
	{
		#region /
		public ActionResult CreateNotification()
		{
			return PartialView();
		}
		#endregion /	

	}
}
