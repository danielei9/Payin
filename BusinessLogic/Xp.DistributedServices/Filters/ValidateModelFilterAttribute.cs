using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Xp.DistributedServices.Filters
{
	public class ValidateModelFilterAttribute : ActionFilterAttribute
	{
		#region OnActionExecuting
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			if (!actionContext.ModelState.IsValid)
				actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
		}
		#endregion OnActionExecuting
	}
}
