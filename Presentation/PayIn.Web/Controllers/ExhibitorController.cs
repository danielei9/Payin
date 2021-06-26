using System;
using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class ExhibitorController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
        #endregion /

        #region /Suspend
        public ActionResult Suspend()
        {
            return PartialView();
        }
        #endregion /Suspend

        #region /Unsuspend
        public ActionResult Unsuspend()
        {
            return PartialView();
        }
        #endregion /Unsuspend

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

        #region /Create
        public ActionResult Create()
        {
            return PartialView();
        }
        #endregion /Create
    }
}