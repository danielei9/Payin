using Newtonsoft.Json.Linq;
using PayIn.Common;
using PayIn.Common.Security;
using PayIn.Web.Models;
using PayIn.Web.Services;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Xp.Common.Resources;

namespace PayIn.Web.Security.Controllers
{
    public class AccountController : Controller
    {
        #region /Login

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

            HttpClientNetwork.Instance.Login(model.Email, model.Password, out HttpResponseMessage resultCode, out JToken resultJSON);
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
                    return View();
            }
            //ASM 20150928
            //Redirect url before asking for authentication!
            if (!string.IsNullOrEmpty(returnUrl))
            {
                if (Helpers.UrlHelper.Instance.IsLocalUrl(returnUrl, out _))
                {
                    return Redirect(returnUrl);
                }
            }
            //ENDASM
            return RedirectToAction("Index", "Home");
        }
        //ENDASM

        #endregion

        #region /Register

        [HttpGet]
        public ActionResult Register(
            int? serviceUser,
            string name,
            string email,
            string phone,
            string photoUrl
        )
        {
            ViewBag.PhotoUrl = photoUrl;

            var model = new RegisterViewModel
            {
                //ServiceUser = serviceUser,
                Name = name ?? "",
                Email = email ?? "",
                Mobile = phone ?? ""
            };
            return View(model);
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
                CheckErrors(ModelState);
                return View(model);
            }

            HttpClientNetwork.Instance.Register(model.Email, model.Name, model.Birthday, model.Mobile, model.Password, model.ConfirmPassword, model.CheckTerms, UserType.User, out HttpResponseMessage resultCode, out JToken resultJSON);
            switch (resultCode.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                    return RedirectToAction("Login", "Account");

            }
            //Error
            if (model.Password.Length < 6) ViewBag.Message = SecurityResources.passwordLengthException;
            else ViewBag.Message = SecurityResources.EmailExists;
            return View();
        }
        //ENDASM

        #endregion

        #region /ConfirmInvitedUser

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ConfirmInvitedUser(
            string userid,
            string code,
            string name,
            string phone,
            string concessionPhotoUrl,
            string concessionName
        )
        {
            ViewBag.ConcessionPhotoUrl = ViewBag.ConcessionPhotoUr ?? concessionPhotoUrl;
            ViewBag.ConcessionName = ViewBag.ConcessionName ?? concessionName;

            var model = new ConfirmInvitedUserViewModel()
            {
                UserId = userid ?? "",
                Code = code ?? "",
                Name = name ?? "",
                Mobile = phone ?? "",
                Password = "",
                ConfirmPassword = ""
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmInvitedUser(ConfirmInvitedUserViewModel model, string returnUrl)
        {
            model.Refresh = false;
            if (!ModelState.IsValid)
                return View(model);

            HttpClientNetwork.Instance.ConfirmEmailAndData(model.UserId, model.Code, model.Mobile, model.Password, out HttpResponseMessage resultCode);
            switch (resultCode.StatusCode)
            {
                case HttpStatusCode.OK:

                case HttpStatusCode.NoContent:
#if VILAMARXANT
					return RedirectToAction("ConfirmedEmailTurismeVilamarxant", "Account");
#elif FINESTRAT
                    return RedirectToAction("ConfirmedEmailFinestrat", "Account");
#else
                    return RedirectToAction("ConfirmedEmail", "Account");
#endif
            }

            return View(model);
        }

        #endregion

        #region /ConfirmInvitedCompany

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ConfirmInvitedCompany()
        {
            //SystemCardMemberCreateInvitedCompanyArguments validateEmail = new SystemCardMemberCreateInvitedCompanyArguments();
            ConfirmInvitedCompanyViewModel validateEmail = new ConfirmInvitedCompanyViewModel();
            validateEmail.Refresh = true;
            if (!string.IsNullOrEmpty(this.Request.Params["userid"]))
                validateEmail.UserId = this.Request.Params["userid"];

            if (!string.IsNullOrEmpty(this.Request.Params["code"]))
                validateEmail.Code = System.Uri.EscapeUriString(this.Request.Params["code"]);

            if (!string.IsNullOrEmpty(this.Request.Params["email"]))
                validateEmail.Login = System.Uri.UnescapeDataString(this.Request.Params["email"]);

            if (!string.IsNullOrEmpty(this.Request.Params["name"]))
                validateEmail.Name = System.Uri.UnescapeDataString(this.Request.Params["name"]);

            validateEmail.Mobile = "";
            validateEmail.Password = "";
            validateEmail.ConfirmPassword = "";

            return View(validateEmail);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //public ActionResult ConfirmInvitedCompany(SystemCardMemberCreateInvitedCompanyArguments model, string returnUrl)
        public ActionResult ConfirmInvitedCompany(ConfirmInvitedCompanyViewModel model, string returnUrlr)
        {
            model.Refresh = false;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            HttpClientNetwork.Instance.ConfirmCompanyEmailAndData(model.UserId, model.Code, model.Mobile, model.Login, model.Password, model.Name, model.TaxName, model.TaxNumber, model.TaxAddress, model.BankAccountNumber, model.Address, model.Observations, out HttpResponseMessage resultCode);
            switch (resultCode.StatusCode)
            {
                case HttpStatusCode.OK:

                case HttpStatusCode.NoContent:


#if VILAMARXANT
					return RedirectToAction("ConfirmedEmailTurismeVilamarxant", "Account");
#elif FINESTRAT
                    return RedirectToAction("ConfirmedEmailFinestrat", "Account");
#else
                    return RedirectToAction("ConfirmedEmail", "Account");
#endif
            }

            return View(model);
        }

        #endregion

        #region /RegisterCompany

        [HttpGet]
        public ActionResult RegisterCompany()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterCompany(RegisterCompanyViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid || !IsValid(model.CheckTerms))
            {
                CheckErrors(ModelState);
                return View(model);
            }

            HttpClientNetwork.Instance.Register(model.Email, model.SupplierName, null, "600 000 000", model.Password, model.ConfirmPassword, model.CheckTerms, model.isBussiness, out HttpResponseMessage resultCode, out JToken resultJSON);
            switch (resultCode.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                    return RedirectToAction("Login", "Account");

            }
            //Error
            if (model.Password.Length < 6) ViewBag.Message = SecurityResources.passwordLengthException;
            else ViewBag.Message = SecurityResources.EmailExists;
            return View();
        }

        #endregion

        #region /ConfirmEmail

        [HttpGet]
        public ActionResult ConfirmEmail()
        {
            ValidateEmailViewModel validateEmail = new ValidateEmailViewModel
            {
                Refresh = true
            };
            if (!string.IsNullOrEmpty(Request.Params["userid"]))
                validateEmail.UserId = Request.Params["userid"];
            if (!string.IsNullOrEmpty(Request.Params["code"]))
                validateEmail.Code = System.Uri.EscapeUriString(Request.Params["code"]);
            return View(validateEmail);
        }

        //ASM 20150925
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmEmail(ValidateEmailViewModel model, string returnUrl)
        {
            model.Refresh = false;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            HttpResponseMessage resultCode = null;
            HttpClientNetwork.Instance.ConfirmEmail(model.UserId, model.Code, out resultCode);
            switch (resultCode.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
#if VILAMARXANT
					return RedirectToAction("ConfirmedEmailTurismeVilamarxant", "Account");
#elif FINESTRAT
					return RedirectToAction("ConfirmedEmailFinestrat", "Account");
#else
                    return RedirectToAction("ConfirmedEmail", "Account");
#endif
            }

            return View(model);
        }
        //ENDASM

        #endregion

        #region /ConfirmedEmail

        [HttpGet]
        public ActionResult ConfirmedEmail(
            string concessionPhotoUrl,
            string concessionName
        )
        {
            ViewBag.ConcessionPhotoUrl = TempData["ConcessionPhotoUrl"] ?? concessionPhotoUrl;
            ViewBag.ConcessionName = TempData["ConcessionName"] ?? concessionName;

            return View();
        }

        #endregion

        #region /WaitEmail

        [HttpGet]
        public ActionResult WaitEmail()
        {
            return View();
        }

        #endregion

        #region /ConfirmedEmailTurismeVilamarxant

        [HttpGet]
        public ActionResult ConfirmedEmailTurismeVilamarxant()
        {
            return View();
        }

        #endregion

        #region /ConfirmedEmailFinestrat

        [HttpGet]
        public ActionResult ConfirmedEmailFinestrat()
        {
            return View();
        }

        #endregion    

        #region /ConfirmedPassword

        [HttpGet]
        public ActionResult ConfirmedPassword()
        {
            return View();
        }

        #endregion

        #region /ConfirmedPasswordTurismeVilamarxant

        [HttpGet]
        public ActionResult ConfirmedPasswordTurismeVilamarxant()
        {
            return View();
        }

        #endregion

        #region /ConfirmedPasswordFinestrat

        [HttpGet]
        public ActionResult ConfirmedPasswordFinestrat()
        {
            return View();
        }

        #endregion

        #region /ForgotPassword

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            HttpResponseMessage resultCode = null;
            HttpClientNetwork.Instance.ForgotPassword(model.Email, out resultCode);
            switch (resultCode.StatusCode)
            {
                case HttpStatusCode.OK:
                    return RedirectToAction("WaitEmail", "Account");
                default:
                    ViewBag.Message = AccountResources.UserException;
                    return View();
            }
        }

        #endregion

        #region /ConfirmForgotPassword

        [HttpGet]
        public ActionResult ConfirmForgotPassword()
        {
            ConfirmForgotPasswordViewModel validateEmail = new ConfirmForgotPasswordViewModel();
            if (!string.IsNullOrEmpty(this.Request.Params["userid"]))
                validateEmail.UserId = this.Request.Params["userid"];
            if (!string.IsNullOrEmpty(this.Request.Params["code"]))
                validateEmail.Code = System.Uri.EscapeUriString(this.Request.Params["code"]).Replace("%20", "+");
            return View(validateEmail);
        }

        //ASM 20150925
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmForgotPassword(ConfirmForgotPasswordViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                CheckErrors(ModelState);
                return View(model);
            }
            HttpResponseMessage resultCode = null;
            HttpClientNetwork.Instance.ConfirmForgotPassword(model.UserId, model.Code, model.Password, model.ConfirmPassword, out resultCode);
            switch (resultCode.StatusCode)
            {
                case HttpStatusCode.OK:
                    return RedirectToAction("ConfirmedPassword", "Account");
            }
            ViewBag.Message = SecurityResources.ValidateConfirmForgotPassword;
            return View();
        }
        //ENDASM

        #endregion

        #region /ConfirmForgotPasswordTurismeVilamarxant

        [HttpGet]
        public ActionResult ConfirmForgotPasswordTurismeVilamarxant()
        {
            ConfirmForgotPasswordViewModel validateEmail = new ConfirmForgotPasswordViewModel();
            if (!string.IsNullOrEmpty(this.Request.Params["userid"]))
                validateEmail.UserId = this.Request.Params["userid"];
            if (!string.IsNullOrEmpty(this.Request.Params["code"]))
                validateEmail.Code = System.Uri.EscapeUriString(this.Request.Params["code"]).Replace("%20", "+");
            return View(validateEmail);
        }

        //ASM 20150925
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmForgotPasswordTurismeVilamarxant(ConfirmForgotPasswordViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                CheckErrors(ModelState);
                return View(model);
            }
            HttpResponseMessage resultCode = null;
            HttpClientNetwork.Instance.ConfirmForgotPassword(model.UserId, model.Code, model.Password, model.ConfirmPassword, out resultCode);
            switch (resultCode.StatusCode)
            {
                case HttpStatusCode.OK:
                    return RedirectToAction("ConfirmedPasswordTurismeVilamarxant", "Account");
            }
            ViewBag.Message = SecurityResources.ValidateConfirmForgotPassword;
            return View();
        }
        //ENDASM

        #endregion

        #region /ConfirmForgotPasswordFinestrat

        [HttpGet]
        public ActionResult ConfirmForgotPasswordFinestrat()
        {
            ConfirmForgotPasswordViewModel validateEmail = new ConfirmForgotPasswordViewModel();
            if (!string.IsNullOrEmpty(this.Request.Params["userid"]))
                validateEmail.UserId = this.Request.Params["userid"];
            if (!string.IsNullOrEmpty(this.Request.Params["code"]))
                validateEmail.Code = System.Uri.EscapeUriString(this.Request.Params["code"]).Replace("%20", "+");
            return View(validateEmail);
        }

        //ASM 20150925
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmForgotPasswordFinestrat(ConfirmForgotPasswordViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                CheckErrors(ModelState);
                return View(model);
            }
            HttpResponseMessage resultCode = null;
            HttpClientNetwork.Instance.ConfirmForgotPassword(model.UserId, model.Code, model.Password, model.ConfirmPassword, out resultCode);
            switch (resultCode.StatusCode)
            {
                case HttpStatusCode.OK:
                    return RedirectToAction("ConfirmedPasswordFinestrat", "Account");
            }
            ViewBag.Message = SecurityResources.ValidateConfirmForgotPassword;
            return View();
        }
        //ENDASM

        #endregion

        #region /UpdatePassword

        [HttpGet]
        public ActionResult UpdatePassword()
        {
            return View();
        }

        #endregion

        #region /ChangePassword

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        #endregion

        #region /Update

        [HttpGet]
        public ActionResult Update()
        {
            return PartialView();
        }

        #endregion

        #region /ImageCrop

        [HttpGet]
        public ActionResult ImageCrop()
        {
            return PartialView();
        }

        #endregion

        #region /GetUsers

        public ActionResult GetUsers()
        {
            return PartialView();
        }

        #endregion

        #region /OverwritePassword

        public ActionResult OverwritePassword()
        {
            return PartialView();
        }

        #endregion

        #region /DeleteImage

        public ActionResult DeleteImage()
        {
            return PartialView();
        }

        #endregion

        #region /Deals

        [HttpGet]
        public ActionResult Deals()
        {
            return PartialView();
        }

        #endregion

        #region /Index

        [HttpGet]
        public ActionResult Index()
        {
            return PartialView();
        }

        #endregion

        #region /UnlockUser

        [HttpGet]
        public ActionResult UnlockUser()
        {
            return PartialView();
        }

        #endregion

        #region Methods

        private void CheckErrors(ModelStateDictionary modelState)
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
