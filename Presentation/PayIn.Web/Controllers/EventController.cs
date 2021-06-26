using System;
using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class EventController : Controller
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

		#region /ImageMenuCrop
		public ActionResult ImageMenuCrop()
		{
			return PartialView();
		}
		#endregion /ImageMenuCrop

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

		#region /Visibility
		public ActionResult Visibility()
		{
			return PartialView();
		}
		#endregion /Visibility

		#region /Show
		public ActionResult Show()
		{
			return PartialView();
		}
		#endregion /Show

		#region /Hide
		public ActionResult Hide()
		{
			return PartialView();
		}
        #endregion /Hide

        #region /AddImageGallery
        public ActionResult AddImageGallery()
        {
            return PartialView();
        }
         #endregion /AddImageGallery

         #region /DeleteImageGallery
         public ActionResult DeleteImageGallery()
             {
                 return PartialView();
             }
        #endregion /DeleteImageGallery

        #region /CreateMapImage
        public ActionResult CreateMapImage()
        {
            return PartialView();
        }
        #endregion /CreateMapImage

        #region /MapImageCrop
        public ActionResult MapImageCrop()
        {
            return PartialView();
        }
        #endregion /MapImageCrop
    }
}