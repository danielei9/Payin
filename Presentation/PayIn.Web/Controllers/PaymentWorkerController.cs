using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class PaymentWorkerController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /

		#region /Create
		public ActionResult Create()
		{
			return PartialView();
		}
		#endregion /Create

		#region /Delete
		public ActionResult Delete()
		{
			return PartialView();
		}
		#endregion /Delete

		#region /InviteUser
		public ActionResult InviteUser()
		{
			return PartialView();
		}
		#endregion /InviteUser

		#region /InvitedUsers
		public ActionResult InvitedUsers()
		{
			return PartialView();
		}
		#endregion /InvitedUsers

		#region /Concession
		public ActionResult Concession()
		{
			return PartialView();
		}
		#endregion /Concession

		#region /DissociateConcession
		public ActionResult DissociateConcession()
		{
			return PartialView();
		}
		#endregion /DissociateConcession

		#region /ResendNotification
		public ActionResult ResendNotification()
		{
			return PartialView();
		}
		#endregion /ResendNotification
	}
}