using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Owin;
using Owin;
using PayIn.Web.Internal.Telemetry;
using System.Web.Http;

[assembly: OwinStartup(typeof(PayIn.Web.Internal.Startup))]
namespace PayIn.Web.Internal
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			var config = new HttpConfiguration();

            TelemetryConfiguration.Active.TelemetryInitializers.Add(new XpTelemetryInitializer());

            app
				.UseSecurity(config)
				.UseWebApi("internal", config)
				.UseWebApi(config)
			;
		}
	}
}
