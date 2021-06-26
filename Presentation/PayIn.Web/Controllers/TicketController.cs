using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class TicketController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
        #endregion /

        #region /SystemCard
        public ActionResult SystemCard()
        {
            return PartialView();
        }
        #endregion /SystemCard

        #region /Create
        public ActionResult CreateTicket()
		{
			return PartialView();
		}
		#endregion /Create

		#region /CreateDetail
		public ActionResult CreateDetail()
		{
			return PartialView();
		}
		#endregion

		#region /Details
		public ActionResult Details()
		{
			return PartialView();
		}
		#endregion /Details

		#region /Update
		public ActionResult Update()
		{
			return PartialView();
		}
        #endregion /Update

        #region /Pay
        public ActionResult Pay()
        {
            return PartialView();
        }
        #endregion /Pay

        #region /UpdateDetail
        public ActionResult UpdateDetail()
		{
			return PartialView();
		}
		#endregion /UpdateDetail

		#region /Delete
		public ActionResult Delete()
		{
			return PartialView();
		}
		#endregion /Delete

		#region /DeleteDetail
		public ActionResult DeleteDetail()
		{
			return PartialView();
		}
		#endregion /DeleteDetail

		#region /Graph
		public ActionResult Graph()
		{
			return PartialView();
		}
		#endregion /Graph

		#region /Sell
		public ActionResult Sell()
		{
			return PartialView();
		}
		#endregion /Sell

		#region /Give
		public ActionResult Give()
		{
			return PartialView();
		}
		#endregion /Give

		#region /RechargeBalance
		public ActionResult RechargeBalance()
		{
			return PartialView();
		}
		#endregion /RechargeBalance

		#region /DonateBalance
		public ActionResult DonateBalance()
		{
			return PartialView();
		}
		#endregion /DonateBalance
	}
}
