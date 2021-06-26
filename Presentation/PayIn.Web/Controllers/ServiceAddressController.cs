using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class ServiceAddressController : Controller
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

		#region /CreateName
		public ActionResult CreateName()
		{
			return PartialView();
		}
		#endregion /CreateName

		#region /UpdateName
		public ActionResult UpdateName()
		{
			return PartialView();
		}
		#endregion /UpdateName

		#region /DeleteName
		public ActionResult DeleteName()
		{
			return PartialView();
		}
		#endregion /DeleteName


	}
}
