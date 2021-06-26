using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class AccountLineController : Controller
    {
        #region /Liquidation
        public ActionResult Liquidation()
        {
            return PartialView();
        }
        #endregion /Liquidation

        #region /LogBook
        public ActionResult LogBook()
        {
            return PartialView();
        }
        #endregion /LogBook

        #region /Accounts
        public ActionResult Accounts()
        {
            return PartialView();
        }
        #endregion /Accounts

        #region /Cash
        public ActionResult Cash()
        {
            return PartialView();
        }
        #endregion /Cash

        #region /ServiceCards
        public ActionResult ServiceCards()
        {
            return PartialView();
        }
        #endregion /ServiceCards

        #region /CreditCards
        public ActionResult CreditCards()
        {
            return PartialView();
        }
        #endregion /CreditCards

        #region /Products
        public ActionResult Products()
        {
            return PartialView();
        }
        #endregion /Products

        #region /EntranceTypes
        public ActionResult EntranceTypes()
        {
            return PartialView();
        }
        #endregion /EntranceTypes

        #region /Others
        public ActionResult Others()
        {
            return PartialView();
        }
        #endregion /Others
    }
}