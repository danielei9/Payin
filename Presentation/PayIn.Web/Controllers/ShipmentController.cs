using System.Web.Mvc;


namespace PayIn.Web.Controllers
{
	public class ShipmentController : Controller
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

		#region /Update
		public ActionResult Update()
		{
			return PartialView();
		}
		#endregion /Update

		#region /Delete
		public ActionResult Delete()
		{
			return PartialView();
		}
		#endregion /Delete

		#region /DeleteTicket
		public ActionResult DeleteTicket()
		{
			return PartialView();
		}
		#endregion /DeleteTicket

		#region /Details
		public ActionResult Details()
		{			
			return PartialView();
		}
		#endregion /Details

		#region /AddUsers
		public ActionResult AddUsers()
		{
			return PartialView();
		}
		#endregion /AddUsers

		#region /Receipt
		public ActionResult Receipt()
		{
			return PartialView();
		}
		#endregion /Receipt
	}
}
