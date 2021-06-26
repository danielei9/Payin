using Newtonsoft.Json.Linq;
using PayIn.Common;
using PayIn.Common.Security;
using PayIn.Web.Models;
using PayIn.Web.Services;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Xp.Common.Resources;

namespace PayIn.Web.Security.Controllers
{
	public class ServiceIncidenceController : Controller
	{
		#region /
		public ActionResult Index()
		{
			return PartialView();
		}
		#endregion /

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
	}
}
