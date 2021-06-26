using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class PurseController : Controller
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

		#region /Users
		public ActionResult Users()
		{
			return PartialView();
		}
		#endregion /Users

		#region CreateImage
		public ActionResult CreateImage()
        {
            return PartialView();
        }
        #endregion CreateImage

        #region /Update
        public ActionResult Update()
		{
			return PartialView();
		}
        #endregion /Update

        #region /ImageCrop
        public ActionResult ImageCrop()
        {
            return PartialView();
        }
        #endregion /ImageCrop

		#region /ImageCropCreate
		public ActionResult ImageCropCreate()
		{
			return PartialView();
		}
		#endregion /ImageCropCreate

        #region /Delete
        public ActionResult Delete()
		{
			return PartialView();
		}
		#endregion /Delete		

		#region /DeleteImage
		public ActionResult DeleteImage()
		{
			return PartialView();
		}
		#endregion /DeleteImage
	}
}
