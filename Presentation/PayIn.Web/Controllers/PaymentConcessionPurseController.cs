using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class PaymentConcessionPurseController : Controller
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

		#region /ResendNotification
		public ActionResult ResendNotification()
		{
			return PartialView();
		}
		#endregion /ResendNotification
	}
}