using System.Web.Mvc;


namespace PayIn.Web.Controllers
{
	public class ControlTemplateCheckController : Controller
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

		#region /Delete
		public ActionResult Delete()
		{
			return PartialView();
		}
		#endregion /Delete

		#region /Update
		public ActionResult Update()
		{
			return PartialView();
		}
		#endregion /Update

		#region /GetAll
		public ActionResult GetAll()
		{
			return PartialView();
		}
		#endregion /GetAll
	}
}