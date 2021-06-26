using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class ControlFormAssignCheckPointController : Controller
	{
		#region /Check
		public ActionResult Check()
		{
			return PartialView();
		}
		#endregion /Check

		#region /Create
		public ActionResult Create()
		{
			return PartialView();
		}
		#endregion /Create

		//#region /Update
		//public ActionResult Update()
		//{
		//	return PartialView();
		//}
		//#endregion /Update

		#region /Delete
		public ActionResult Delete()
		{
			return PartialView();
		}
		#endregion /Delete
	}
}
