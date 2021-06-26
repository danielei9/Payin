using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class ControlTemplateItemController : Controller
	{
		#region /Template
		public ActionResult Template()
		{
			return PartialView();
		}
		#endregion /Template

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
	}
}