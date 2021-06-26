using Microsoft.ApplicationInsights.Extensibility;
using PayIn.Web.Telemetry;
using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PayIn.Web
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			//GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			// removes duplicate X-Frame-Options header
			// Esto es necesario para que una página web sea mostrada dentro de un iFrame en el móvil o en otra web
			// AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
		}
		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			var cultureName = CultureInfo.InstalledUICulture.IetfLanguageTag;
			if (HttpContext.Current != null && HttpContext.Current.Request.UserLanguages != null)
				cultureName = Request.UserLanguages[0];

			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
			Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName); 
		}
	}
}
