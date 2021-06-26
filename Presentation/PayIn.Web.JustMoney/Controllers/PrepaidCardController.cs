using System.Web.Mvc;

namespace PayIn.Web.JustMoney.Controllers
{
	public class PrepaidCardController : Controller
	{
		#region PrepaidCard/
		[HttpGet]
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion PrepaidCard/

		#region PrepaidCard/Create
		[HttpGet]
		public ActionResult Create()
		{
			return PartialView();
		}
		#endregion PrepaidCard/Create

		#region PrepaidCard/CreateAndRegister
		[HttpGet]
		public ActionResult CreateAndRegister()
		{
			return PartialView();
		}
		#endregion PrepaidCard/RegiCreateAndRegisterster

		#region PrepaidCard/AddCard
		[HttpGet]
		public ActionResult AddCard()
		{
			return PartialView();
		}
		#endregion PrepaidCard/AddCard

		#region PrepaidCard/CreateCard
		[HttpGet]
		public ActionResult CreateCard()
		{
			return PartialView();
		}
		#endregion PrepaidCard/CreateCard

		#region PrepaidCard/RegisterCard
		[HttpGet]
		public ActionResult RegisterCard()
		{
			return PartialView();
		}
		#endregion PrepaidCard/RegisterCard

		#region PrepaidCard/EnableDisable
		[HttpGet]
		public ActionResult EnableDisable()
		{
			return PartialView();
		}
		#endregion PrepaidCard/EnableDisable

		#region PrepaidCard/LoadFunds
		[HttpGet]
		public ActionResult LoadFunds()
		{
			return PartialView();
		}
		#endregion PrepaidCard/LoadFunds

		#region PrepaidCard/Log
		[HttpGet]
		public ActionResult Log()
		{
			return PartialView();
		}
		#endregion PrepaidCard/Log

		#region PrepaidCard/PointsOfRecharge
		[HttpGet]
		public ActionResult PointsOfRecharge()
		{
			return PartialView();
		}
		#endregion PrepaidCard/PointsOfRecharge

		#region PrepaidCard/RechargeCard
		[HttpGet]
		public ActionResult RechargeCard()
		{
			return PartialView();
		}
		#endregion PrepaidCard/RechargeCard

		#region PrepaidCard/RechargedCard
		[HttpGet]
		public ActionResult RechargedCard()
		{
			return PartialView();
		}
		#endregion PrepaidCard/RechargedCard

		#region PrepaidCard/RechargedCardError
		[HttpGet]
		public ActionResult RechargedCardError()
		{
			return PartialView();
		}
		#endregion PrepaidCard/RechargedCardError

		#region PrepaidCard/ShareFunds
		[HttpGet]
		public ActionResult ShareFunds()
		{
			return PartialView();
		}
		#endregion PrepaidCard/ShareFunds

		#region PrepaidCard/Transfer
		[HttpGet]
		public ActionResult Transfer()
		{
			return PartialView();
		}
		#endregion PrepaidCard/Transfer

		#region PrepaidCard/Update
		[HttpGet]
		public ActionResult Update()
		{
			return PartialView();
		}
		#endregion PrepaidCard/Update

		#region PrepaidCard/Upgrade
		[HttpGet]
		public ActionResult Upgrade()
		{
			return PartialView();
		}
		#endregion PrepaidCard/Upgrade
	}
}