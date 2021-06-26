using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
    // Esta clase no puede estar securizada porque en ella se carga Angular y la identidad del sessionStorage
    public class HomeController : Controller
    {
        #region GET /

        public ActionResult Index()
        {
            //ASM 20150922 
            if (TempData["accessToken"] != null)
            {
                ViewBag.AccessToken = "window.sessionStorage.setItem('accessToken', '" + TempData["accessToken"] + "');";
                ViewBag.RefreshToken = "window.sessionStorage.setItem('refreshToken', '" + TempData["refreshToken"] + "');";
                ViewBag.UserName = "window.sessionStorage.setItem('userName', '" + TempData["userName"] + "');";
                ViewBag.Roles = "window.sessionStorage.setItem('roles', '" + TempData["roles"] + "');";
                ViewBag.ClientId = "window.sessionStorage.setItem('clientId', '" + TempData["clientId"] + "');";

                // Esto se usaba porqeu los falleres eran redirigios a la web de FACCA
                //#if FALLAS
                //				ViewBag.CanAccessUsers = false;
                //#else
                //				ViewBag.CanAccessUsers = true;
                //#endif
            }
            //ENDASM 20150922
            return View();
        }

        #endregion

        #region GET / Deals

        public ActionResult Deals()
        {
            return PartialView();
        }

        #endregion

        #region GET / Popup

        public ActionResult Popup()
        {
            return PartialView();
        }

        #endregion
    }
}
