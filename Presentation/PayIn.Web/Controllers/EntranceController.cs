using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class EntranceController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /

		#region Invite
		public ActionResult Invite()
		{
			return PartialView();
		}
        #endregion Invite

        #region Validation
        public ActionResult Validation()
        {
            return PartialView();
        }
        #endregion Validation

        #region Mail
        public ActionResult Mail()
        {
            return PartialView();
        }
		#endregion Mail

		#region /Suspend
		public ActionResult Suspend()
		{
			return PartialView();
		}
		#endregion /Suspend

		#region /Unsuspend
		public ActionResult Unsuspend()
		{
			return PartialView();
		}
		#endregion /Unsuspend

		#region /EntranceTicket
		public ActionResult EntranceTicket()
		{
			return PartialView();
		}
		#endregion /EntranceTicket

		#region /ChangeCard
		public ActionResult ChangeCard()
		{
			return PartialView();
		}
		#endregion /ChangeCard
	}
}
