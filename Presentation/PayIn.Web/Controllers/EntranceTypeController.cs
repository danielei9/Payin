using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class EntranceTypeController : Controller
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

        #region /Relocate
        public ActionResult Relocate()
        {
            return PartialView();
        }
		#endregion /Relocate

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

		#region /Visibility
		public ActionResult Visibility()
		{
			return PartialView();
		}
		#endregion /Visibility

		#region /Groups
		public ActionResult Groups()
		{
			return PartialView();
		}
		#endregion /Groups
		
		#region /AddGroup
		public ActionResult AddGroup()
		{
			return PartialView();
		}
		#endregion /AddGroup

		#region /RemoveGroup
		public ActionResult RemoveGroup()
		{
			return PartialView();
		}
		#endregion /RemoveGroup

		#region /GetSellable
		public ActionResult GetSellable()
		{
			return PartialView();
		}
        #endregion /GetSellable

        #region /GetBuyable
        public ActionResult GetBuyable()
        {
            return PartialView();
        }
        #endregion /GetBuyable

        #region /GetToGive
        public ActionResult GetToGive()
		{
			return PartialView();
		}
		#endregion /GetToGive

		#region /Recharge
		public ActionResult Recharge()
		{
			return PartialView();
		}
		#endregion /Recharge

		#region /Donate
		public ActionResult Donate()
		{
			return PartialView();
		}
		#endregion /Donate

		#region /BuyRecharge
		public ActionResult BuyRecharge()
		{
			return PartialView();
		}
		#endregion /BuyRecharge
	}
}