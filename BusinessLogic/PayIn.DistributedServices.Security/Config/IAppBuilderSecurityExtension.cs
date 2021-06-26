using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using PayIn.DistributedServices.Security.Providers;
using System;
using System.Web.Http;

namespace Owin
{
	public static class IAppBuilderSecurityExtension
	{
		#region UseSecurity

		public static IAppBuilder UseSecurity(this IAppBuilder app, HttpConfiguration config)
		{
			var oAuthBearerOptions = new OAuthBearerAuthenticationOptions();

			app.UseOAuthBearerAuthentication(oAuthBearerOptions);
			return app;
		}
		public static IAppBuilder UseSecurity(this IAppBuilder app, string path, HttpConfiguration config)
		{
			var oAuthBearerOptions = new OAuthBearerAuthenticationOptions()
			{
				Provider = new XpOAuthBearerAuthenticationProvider()
			};

			var oAuthOptions = new OAuthAuthorizationServerOptions
			{
				AllowInsecureHttp = true,
				TokenEndpointPath = new PathString("/" + path),
				AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
				Provider = new XpOAuthAuthorizationServerProvider(),
				RefreshTokenProvider = new XpAuthenticationTokenProvider()
			};

			config.SuppressDefaultHostAuthentication();
			config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
			//config.Formatters.Insert(0, new MultiFormDataMediaTypeFormatter());
			//config.Formatters.Insert(0, new BinaryMediaTypeFormatter());

			// Token generation
			app.UseOAuthAuthorizationServer(oAuthOptions);
			app.UseOAuthBearerAuthentication(oAuthBearerOptions);
			app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.ExternalCookie, TimeSpan.FromMinutes(5));
			return app;
		}
		#endregion UseSecurity
	}
}
