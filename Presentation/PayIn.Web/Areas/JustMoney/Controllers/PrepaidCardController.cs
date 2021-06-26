using System.Web.Mvc;

namespace PayIn.Web.Areas.JustMoney.Controllers
{
	public class PrepaidCardController : Controller
    {
		#region GET JustMoney/PrepaidCard
		public ActionResult Index()
        {
            return PartialView();
		}
		#endregion GET JustMoney/PrepaidCard

		#region GET JustMoney/PrepaidCard/Activate
		public ActionResult Activate()
		{
			return PartialView();
		}
		#endregion GET JustMoney/PrepaidCard/Activate

		#region GET JustMoney/PrepaidCard/Create
		public ActionResult Create()
		{
			return PartialView();
		}
		#endregion GET JustMoney/PrepaidCard/Create
	}
}