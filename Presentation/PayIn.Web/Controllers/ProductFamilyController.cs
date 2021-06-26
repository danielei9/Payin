using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class ProductFamilyController : Controller
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
	}
}
