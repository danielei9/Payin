using System.Web.Mvc;

namespace PayIn.Web.Security.Controllers
{
	public class ServiceCardController : Controller
	{
		#region /
		[HttpGet]
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /

		#region /MyCards
		[HttpGet]
		public ActionResult MyCards()
		{
			return PartialView();
		}
		#endregion /MyCards

		#region /MyCardsPrev
		[HttpGet]
		public ActionResult MyCardsPrev()
		{
			return PartialView();
		}
		#endregion /MyCardsPrev

		#region /Item
		[HttpGet]
		public ActionResult Item()
		{
			return PartialView();
		}
		#endregion /Item

		#region /Update
		public ActionResult Update()
		{
			return PartialView();
		}
		#endregion /Update

		#region /Create
		[HttpGet]
		public ActionResult Create()
		{
			return PartialView();
		}
		#endregion /Create

		#region /Destroy
		[HttpGet]
		public ActionResult Destroy()
		{
			return PartialView();
		}
		#endregion /Destroy

		#region /Lock
		[HttpGet]
		public ActionResult Lock()
		{
			return PartialView();
		}
        #endregion /Lock
        
        #region /Unlock
        [HttpGet]
        public ActionResult Unlock()
        {
            return PartialView();
        }
		#endregion /Unlock

		#region /LinkCard
		[HttpGet]
		public ActionResult LinkCard()
		{
			return PartialView();
		}
		#endregion /LinkCard

		#region /UnlinkCard
		[HttpGet]
		public ActionResult UnlinkCard()
		{
			return PartialView();
		}
		#endregion /UnlinkCard	
	}
}