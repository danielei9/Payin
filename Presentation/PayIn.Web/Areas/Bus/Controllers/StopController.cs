using System.Web.Mvc;

namespace PayIn.Web.Areas.Bus.Controllers
{
	public class StopController : Controller
    {
        #region GET Bus/Stop/Create

        public ActionResult Create()
        {
            return PartialView();
        }
        #endregion GET Bus/Stop/Create

        #region GET Bus/Stop/Update
        public ActionResult Update()
		{
			return PartialView();
		}
		#endregion GET Bus/Stop/Update

		#region GET Bus/Stop/UpdateLink
		public ActionResult UpdateLink()
		{
			return PartialView();
		}
		#endregion GET Bus/Stop/UpdateLink

		#region GET Bus/Stop/ByLine
		public ActionResult ByLine()
        {
            return PartialView();
        }
		#endregion GET Bus/Stop/ByLine

		#region GET Bus/Stop/Visit
		public ActionResult Visit()
		{
			return PartialView();
		}
		#endregion GET Bus/Stop/Visit
	}
}