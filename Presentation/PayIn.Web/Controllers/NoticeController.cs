using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class NoticeController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
        #endregion /

        #region /Pages
        public ActionResult Pages()
        {
            return PartialView();
        }
        #endregion /Pages

        #region /Edicts
        public ActionResult Edicts()
        {
            return PartialView();
        }
        #endregion /Edicts

        #region /Create
        public ActionResult Create()
		{
			return PartialView();
		}
        #endregion /Create

        #region /CreatePage
        public ActionResult CreatePage()
        {
            return PartialView();
        }
        #endregion /CreCreatePageate

        #region /CreateEdict
        public ActionResult CreateEdict()
        {
            return PartialView();
        }
        #endregion /CreateEdict

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

        #region /UpdatePage
        public ActionResult UpdatePage()
        {
            return PartialView();
        }
        #endregion /UpdatePage

        #region /UpdateEdict
        public ActionResult UpdateEdict()
        {
            return PartialView();
        }
        #endregion /UpdateEdict

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
	}
}