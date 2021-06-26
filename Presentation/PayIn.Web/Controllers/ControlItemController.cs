using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class ControlItemController : Controller
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

		#region /AddTag
		public ActionResult AddTag()
		{
			return PartialView();
		}
		#endregion /AddTag

		#region /RemoveTag
		public ActionResult RemoveTag()
		{
			return PartialView();
		}
		#endregion /RemoveTag
	}
}