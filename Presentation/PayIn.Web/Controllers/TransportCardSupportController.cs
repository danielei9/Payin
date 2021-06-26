using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class TransportCardSupportController : Controller
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

	}
}
