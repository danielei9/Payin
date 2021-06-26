using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class CampaignLineController : Controller
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

		#region /GetByProduct
		public ActionResult GetByProduct()
		{
			return PartialView();
		}
		#endregion /GetByProduct

		#region /AddProduct
		public ActionResult AddProduct()
		{
			return PartialView();
		}
		#endregion /AddProduct

		#region /RemoveProduct
		public ActionResult RemoveProduct()
		{
			return PartialView();
		}
		#endregion /RemoveProduct

		#region /GetByProductFamily
		public ActionResult GetByProductFamily()
		{
			return PartialView();
		}
		#endregion /GetByProductFamily

		#region /AddProductFamily
		public ActionResult AddProductFamily()
		{
			return PartialView();
		}
		#endregion /AddProductFamily

		#region /RemoveProductFamily
		public ActionResult RemoveProductFamily()
		{
			return PartialView();
		}
		#endregion /RemoveProductFamily

		#region /GetByServiceUser
		public ActionResult GetByServiceUser()
		{
			return PartialView();
		}
		#endregion /GetByServiceUser

		#region /AddServiceUser
		public ActionResult AddServiceUser()
		{
			return PartialView();
		}
		#endregion /AddServiceUser

		#region /RemoveServiceUser
		public ActionResult RemoveServiceUser()
		{
			return PartialView();
		}
		#endregion /RemoveServiceUser
		 
		#region /GetByServiceGroup
		public ActionResult GetByServiceGroup()
		{
			return PartialView();
		}
		#endregion /GetByServiceGroup

		#region /AddServiceGroup
		public ActionResult AddServiceGroup()
		{
			return PartialView();
		}
		#endregion /AddServiceGroup

		#region /RemoveServiceGroup
		public ActionResult RemoveServiceGroup()
		{
			return PartialView();
		}
		#endregion /RemoveServiceGroup

		#region /GetByEntranceType
		public ActionResult GetByEntranceType()
		{
			return PartialView();
		}
		#endregion /GetByEntranceType

		#region /AddEntranceType
		public ActionResult AddEntranceType()
		{
			return PartialView();
		}
		#endregion /AddEntranceType

		#region /RemoveEntranceType
		public ActionResult RemoveEntranceType()
		{
			return PartialView();
		}
		#endregion /RemoveEntranceType
		
	}
}