using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class ProductController : Controller
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

		#region /UpdateCard
		public ActionResult UpdateCard()
		{
			return PartialView();
		}
		#endregion /UpdateCard

		#region /UpdateCardSelect
		public ActionResult UpdateCardSelect()
		{
			return PartialView();
		}
		#endregion /UpdateCardSelect

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

		#region /Productdashboard
		public ActionResult ProductDashboard()
		{
			return PartialView();
		}
		#endregion /Productdashboard
		
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
	}
}
