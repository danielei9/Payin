using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class CampaignController : Controller
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

		#region /CreateImage
		public ActionResult CreateImage()
		{
			return PartialView();
		}
		#endregion /CreateImage

		#region /ImageCrop
		public ActionResult ImageCrop()
		{
			return PartialView();
		}
		#endregion /ImageCrop

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

		#region /EventList
		public ActionResult EventList()
        {
            return PartialView();
        }
		#endregion /EventList

		#region /AddEvent
		public ActionResult AddEvent()
        {
            return PartialView();
        }
        #endregion /AddEvent

        #region /RemoveEvent
        public ActionResult RemoveEvent()
        {
            return PartialView();
        }
        #endregion /RemoveEvent

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
	}
}
