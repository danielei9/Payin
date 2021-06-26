using System.Web.Mvc;

namespace PayIn.Web.JustMoney.Controllers
{
	public class AccountController : Controller
	{
		[HttpGet]
		public ActionResult Login()
		{
			return PartialView();
		}

		[HttpGet]
		public ActionResult ForgotPassword()
		{
			return PartialView();
		}

		[HttpGet]
		public ActionResult ConfirmForgotPassword()
		{
			return PartialView();
		}
	}
}