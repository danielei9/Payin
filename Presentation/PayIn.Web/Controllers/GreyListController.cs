using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class GreyListController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /

		#region /Delete
		public ActionResult Delete()
		{
			return PartialView();
		}
		#endregion /Delete

		#region /ReadAll
		public ActionResult ReadAll()
		{
			return PartialView();
		}
		#endregion /ReadAll

		#region /ModifyBlock
		public ActionResult ModifyBlock()
		{
			return PartialView();
		}
		#endregion /ModifyBlock
	}
}
