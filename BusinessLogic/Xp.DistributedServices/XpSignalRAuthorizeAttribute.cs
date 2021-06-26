using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Xp.Common;
using Xp.Common.Resources;

namespace PayIn.Web.Security
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class XpSignalRAuthorizeAttribute : AuthorizeAttribute
	{
		public string ClientIds { get; set; }

		#region AuthorizeHubConnection
		public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
		{
			if (!ClientIds.IsNullOrEmpty())
			{
				var MethodClientIds = ClientIds.SplitString(",");

				var claims = (Thread.CurrentPrincipal as ClaimsPrincipal).Claims;
				var userClientIds = claims
					.Where(x => x.Type == XpClaimTypes.ClientId)
					.Select(x => x.Value);

				if (!userClientIds.Intersect(MethodClientIds).Any())
					throw new UnauthorizedAccessException(AccountResources.ClientIdNotAllowedException.FormatString(userClientIds.JoinString(",")));
			}

			var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
			identity.AddClaim(new Claim(XpClaimTypes.Uri, request.Url.AbsoluteUri));
			identity.AddClaim(new Claim(XpClaimTypes.Token, request.QueryString.Get("bearer")));

			return base.AuthorizeHubConnection(hubDescriptor, request);
		}
		#endregion AuthorizeHubConnection
	}
}
