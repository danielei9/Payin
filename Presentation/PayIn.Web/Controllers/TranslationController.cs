using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
	public class TranslationController : Controller
	{

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

		#region /UpdateFormattedText
		public ActionResult UpdateFormattedText()
		{
			return PartialView();
		}
		#endregion /UpdateFormattedText

		#region /CreateFormattedText
		public ActionResult CreateFormattedText()
		{
			return PartialView();
		}
		#endregion /CreateFormattedText
	}
}
