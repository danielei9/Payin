using System.Web.Mvc;

namespace PayIn.Web.Areas.JustMoney.Controllers
{
	public class UserController : Controller
    {
		#region GET JustMoney/User
		public ActionResult Index()
        {
            return PartialView();
		}
		#endregion GET JustMoney/User

		//#region GET JustMoney/User/Details
		//public ActionResult Details()
		//{
		//	return PartialView();
		//}
		//#endregion GET JustMoney/User/Details

		#region GET JustMoney/User/Update
		public ActionResult Update()
		{
			return PartialView();
		}
		#endregion GET JustMoney/User/Update
	}
}