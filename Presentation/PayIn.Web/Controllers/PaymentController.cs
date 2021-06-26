using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class PaymentController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /

		#region /Charges
		public ActionResult Charges()
		{
			return PartialView();
		}
		#endregion /Charges

		#region /PaymentDetails
		public ActionResult PaymentDetails()
		{
			return PartialView();
		}
		#endregion /PaymentDetails

		#region /Graph
		public ActionResult Graph()
		{
			return PartialView();
		}
		#endregion /Graph

		#region /LiquidationPayments
		public ActionResult LiquidationPayments()
		{
			return PartialView();
		}
		#endregion /LiquidationPayments

		#region /LiquidationNull
		public ActionResult LiquidationNull()
		{
			return PartialView();
		}
		#endregion /LiquidationNull

		#region /Refund
		public ActionResult Refund()
		{
			return PartialView();
		}
		#endregion /Refund

		#region /Unliquidated
		public ActionResult Unliquidated()
		{
			return PartialView();
		}
		#endregion /Unliquidated

		#region /CardPayments
		public ActionResult CardPayments()
		{
			return PartialView();
		}
		#endregion /CardPayments

	}
}