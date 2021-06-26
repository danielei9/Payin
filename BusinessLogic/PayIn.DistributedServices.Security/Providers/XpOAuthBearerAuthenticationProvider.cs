using Microsoft.Owin.Security.OAuth;
using System;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Security.Providers
{
	public class XpOAuthBearerAuthenticationProvider
	: OAuthBearerAuthenticationProvider
	{
		#region RequestToken
		public override Task RequestToken(OAuthRequestTokenContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");

			var tokenCookie = context.OwinContext.Request.Query.Get("bearer");
			if (!string.IsNullOrEmpty(tokenCookie))
				context.Token = tokenCookie;

			return base.RequestToken(context);
		}
		#endregion RequestToken
	}
}
