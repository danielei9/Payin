using Newtonsoft.Json.Linq;
using PayIn.Common;
using PayIn.Common.Security;
using PayIn.Web.Models;
using PayIn.Web.Services;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Xp.DistributedServices.Filters;

namespace PayIn.Web.Controllers
{
    public class ShopController : Controller
	{
        #region GET /
        [HttpGet]
        public ActionResult Index()
		{
			if (TempData["accessToken"] != null)
			{
				ViewBag.AccessToken = "window.sessionStorage.setItem('accessToken', '" + TempData["accessToken"] + "');";
				ViewBag.RefreshToken = "window.sessionStorage.setItem('refreshToken', '" + TempData["refreshToken"] + "');";
				ViewBag.UserName = "window.sessionStorage.setItem('userName', '" + TempData["userName"] + "');";
				ViewBag.Roles = "window.sessionStorage.setItem('roles', '" + TempData["roles"] + "');";
				ViewBag.ClientId = "window.sessionStorage.setItem('clientId', '" + TempData["clientId"] + "');";
			}

			return View();
		}
        #endregion GET /

        #region GET /Concession
        [HttpGet]
        public ActionResult Concession()
        {
            return View();
        }
		#endregion GET /Concession

		#region GET /ByConcession
		[HttpGet]
		public ActionResult ByConcession()
		{
			return View();
		}
		#endregion GET /ByConcession

		#region GET /Form
		[HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        #endregion GET /Form

        #region GET /Event
        [HttpGet]
        public ActionResult Event()
        {
            return View();
        }
        #endregion GET /Event

        #region GET /Entrance
        [HttpGet]
        public ActionResult Entrance()
        {
            return View();
        }
        #endregion GET /Entrance

        #region GET /View
        [HttpGet]
        public ActionResult All()
        {
            return View();
        }
        #endregion GET /View

        #region GET /LoginCorrect
        [HttpGet]
        public ActionResult Successful()
        {
            return View();
        }
        #endregion GET /LoginCorrect

        #region GET /LoginIncorrect
        [HttpGet]
        public ActionResult Wrong()
        {
            return View();
        }
        #endregion GET /LoginIncorrect

        #region GET /Login
        [HttpGet]
        public ActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View();
        }

        //ASM 20150921
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            HttpResponseMessage resultCode = null;
            JToken resultJSON = "";
            HttpClientNetwork.Instance.Login(model.Email, model.Password, out resultCode, out resultJSON);
            switch (resultCode.StatusCode)
            {
                case HttpStatusCode.OK:
                    TempData["accessToken"] = resultJSON["access_token"];
                    TempData["refreshToken"] = resultJSON["refresh_token"];
                    TempData["userName"] = resultJSON["as:name"];
                    TempData["roles"] = resultJSON["as:roles"];
                    TempData["clientId"] = resultJSON["as:client_id"];
                    break;
                default: //Error
                    ViewBag.Message = resultJSON["error_description"];
                    //return View();
                    return RedirectToAction("Wrong");
            }
            //ASM 20150928
            //Redirect url before asking for authentication!
            if (!string.IsNullOrEmpty(returnUrl))
            {
                string sBaseUrl = "";
                if (PayIn.Web.Helpers.UrlHelper.Instance.IsLocalUrl(returnUrl, out sBaseUrl))
                {
                    return Redirect(returnUrl);
                }
            }
            //ENDASM
            return RedirectToAction("Successful");
        }
        //ENDASM
        #endregion GET /Login

        #region /Register
        [HttpGet]
        //[XpXFrameOptions()]
        public ActionResult Register()
        {
            return View();
        }

        //ASM 20150928
        public bool IsValid(object value)
        {
            return value != null && (bool)value == true;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid || !IsValid(model.CheckTerms))
            {
                checkErrors(ModelState);
                return View(model);
            }

            HttpResponseMessage resultCode = null;
            JToken resultJSON = "";
            HttpClientNetwork.Instance.Register(model.Email, model.Name, model.Birthday, model.Mobile, model.Password, model.ConfirmPassword, model.CheckTerms, UserType.User, out resultCode, out resultJSON);
            switch (resultCode.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                    return RedirectToAction("Login", "Shop");
                case HttpStatusCode.InternalServerError:
                    return RedirectToAction("Wrong");

            }
            //Error
            if (model.Password.Length < 6) ViewBag.Message = SecurityResources.passwordLengthException;
            else ViewBag.Message = SecurityResources.EmailExists;
            return View();
        }

        //ENDASM
        #endregion /Register

        #region methods
        private void checkErrors(ModelStateDictionary modelState)
        {
            try
            {
                ViewBag.Message = modelState.Values.SelectMany(v => v.Errors).First<ModelError>().ErrorMessage;
            }
            catch
            {
                ViewBag.Message = SecurityResources.CheckAcceptTerms;
            }
        }
        #endregion
    }
}
