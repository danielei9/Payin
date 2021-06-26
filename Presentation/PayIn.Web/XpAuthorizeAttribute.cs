using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PayIn.Web.Security
{
	[AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class XpAuthorizeAttribute : AuthorizeAttribute
	{
		public string ClientIds { get; set; }
 
		#region HandleUnauthorizedRequest
    //Called when access is denied
    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			//User isn't logged in
			if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
			{
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
				return;
			}

			//User is logged in but has no access
			filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "NotAuthorized" }));
    }
		#endregion HandleUnauthorizedRequest
 
		#region AuthorizeCore
    protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			if (!ClientIds.IsNullOrEmpty())
			{
				var clientIdList = ClientIds.SplitString(",");
			}

			//var b = myMembership.Instance.Member().IsLoggedIn;
			////Is user logged in?
			//if ( b )
			//		//If user is logged in and we need a custom check:
			//		if ( ResourceKey != null && OperationKey != null )
			//				return ecMembership.Instance.Member().ActivePermissions.Where( x => x.operation == OperationKey && x.resource == ResourceKey ).Count() > 0;
			////Returns true or false, meaning allow or deny. False will call HandleUnauthorizedRequest above
			//return b;
			return base.AuthorizeCore(httpContext);
		}
		#endregion AuthorizeCore
	}
}
