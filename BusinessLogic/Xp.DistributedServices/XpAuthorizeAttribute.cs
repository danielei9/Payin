using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using Xp.Common;
using Xp.Common.Resources;

namespace PayIn.Web.Security
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class XpAuthorizeAttribute : AuthorizeAttribute
	{
		public string ClientIds { get; set; }

		#region OnAuthorization
		public override void OnAuthorization(HttpActionContext actionContext)
		{
			base.OnAuthorization(actionContext);

			var principal = Thread.CurrentPrincipal as ClaimsPrincipal;
			if (!principal.Identity.IsAuthenticated)
				throw new UnauthorizedAccessException();

			if (!ClientIds.IsNullOrEmpty())
			{
				var MethodClientIds = ClientIds.SplitString(",");
				
                var claims = principal.Claims;
				var userClientIds = claims
					.Where(x => x.Type == XpClaimTypes.ClientId)
					.Select(x => x.Value);

				if (!userClientIds.Intersect(MethodClientIds).Any())
					throw new UnauthorizedAccessException(AccountResources.ClientIdNotAllowedException.FormatString(userClientIds.JoinString(",")));
			}

			var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
			identity.AddClaim(new Claim(XpClaimTypes.Uri, actionContext.Request.RequestUri.AbsoluteUri));
			identity.AddClaim(new Claim(XpClaimTypes.Token, actionContext.Request.Headers.Authorization.Parameter));
		}
		#endregion OnAuthorization
	}
}
