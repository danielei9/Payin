using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace PayIn.Web.JustMoney
{
	public class Global : HttpApplication
    {
		protected void Application_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta al iniciar la aplicación
            AreaRegistration.RegisterAllAreas();
			//GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			//BundleConfig.RegisterBundles(BundleTable.Bundles);
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