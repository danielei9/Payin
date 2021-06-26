//using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Owin;
using Owin;
//using PayIn.JustMoney.Telemetry;
using System.Web.Http;

[assembly: OwinStartup(typeof(PayIn.JustMoney.Startup))]
namespace PayIn.JustMoney
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			var config = new HttpConfiguration();

			//TelemetryConfiguration.Active.TelemetryInitializers.Add(new XpTelemetryInitializer());

			//config
			//	.EnableSwagger(c =>
			//	{
			//		c.SingleApiVersion("v1", "Pay[in] Api")
			//			.Description("Api de Pay[in] para la integración de otros sistemas.")
			//			.TermsOfService("Para poder acceder a estos puntos de acceso es necesario la contratación de este acceso.")
			//			.Contact(cc => cc
			//				.Name("Pay[in]")
			//				.Email("info@pay-in.es")
			//				.Url("http://www.pay-in.es")
			//			)
			//			.License(lc => lc
			//				.Name("Licencia de uso")
			//				.Url("http://www.pay-in.es"));

			//		c.DocumentFilter<HideSwaggerFilter>();
			//		c.DocumentFilter<AddEnumDescriptionsSwaggerFilter>();

			//		c.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + @"\Content\PayIn.DistributedServices.Payments.XML");
			//		c.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + @"\Content\PayIn.DistributedServices.Transport.XML");
			//		c.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + @"\Content\PayIn.DistributedServices.SmartCity.XML");
			//		c.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + @"\Content\PayIn.DistributedServices.XML");
			//	})
			//	.EnableSwaggerUi(c => {
			//	});

			app
				.UseWebApi("api", config)
			;

			//// Temporal SATEL
			//config.Formatters.Insert(0, new ChangeMediaTypeFormatterFormatter(
			//	config.Formatters,
			//	"application/x-www-form-urlencoded",
			//	"application/json",
			//	new Type[] {
			//		typeof(SentiloDataUpdateProviderArguments),
			//		typeof(SentiloDataUpdateSensorArguments),
			//		typeof(SentiloAlertUpdateArguments),
			//		typeof(SentiloDataUpdateValueArguments)
			//	}
			//));
		}
	}
}