using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class ServiceCheckPointController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /		

		#region /Item
		public ActionResult Item()
		{
			return PartialView();
		}
		#endregion /Item

		#region /IndexCheckPoint
		public ActionResult IndexCheckPoint()
		{
			return PartialView();
		}
		#endregion /IndexCheckPoint
		
		#region /Create
		public ActionResult Create()
		{
			return PartialView();
		}
		#endregion /Create	

		//#region /CreateCheckPoint
		//public ActionResult CreateCheckPoint()
		//{
		//	return PartialView();
		//}
		//#endregion /CreateCheckPoint

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
