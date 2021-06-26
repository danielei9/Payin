using System.Web.Mvc;


namespace PayIn.Web.Controllers
{
	public class LiquidationController : Controller
	{
		#region /Index
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /Index

		#region /Create
		public ActionResult Create()
		{
			return PartialView();
		}
		#endregion /Create

		#region /Liquidations
		public ActionResult Liquidations()
		{
			return PartialView();
		}
		#endregion /Liquidations

		#region /Change
		public ActionResult Change()
		{
			return PartialView();
		}
        #endregion /Change

        #region /CreateAndPay
        public ActionResult CreateAndPay()
        {
            return PartialView();
        }
        #endregion /CreateAndPay

        #region /Open
        public ActionResult Open()
		{
			return PartialView();
		}
        #endregion /Open

        #region /Close
        public ActionResult Close()
        {
            return PartialView();
        }
        #endregion /Close

        #region /Pay
        public ActionResult Pay()
		{
			return PartialView();
		}
		#endregion /Pay

        #region /CreateAccountLines
        public ActionResult CreateAccountLines()
        {
            return PartialView();
        }
        #endregion /CreateAccountLines

        #region /AddAccountLines
        public ActionResult AddAccountLines()
        {
            return PartialView();
        }
        #endregion /AddAccountLines

        #region /RemoveAccountLines
        public ActionResult RemoveAccountLines()
        {
            return PartialView();
        }
        #endregion /RemoveAccountLines
    }
}