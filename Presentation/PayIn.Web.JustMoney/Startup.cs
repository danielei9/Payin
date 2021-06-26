//using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Owin;
using Owin;
//using PayIn.Application.Dto.SmartCity.Arguments;
//using PayIn.Web.Telemetry;
//using Swashbuckle.Application;
//using System;
using System.Web.Http;
//using Xp.DistributedServices.Filters;
//using Xp.DistributedServices.Formatters;

[assembly: OwinStartup(typeof(PayIn.Web.JustMoney.Startup))]
namespace PayIn.Web.JustMoney
{
	public partial class Startup
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
			//		//c.RootUrl((x) => "http://payin-test.cloudapp.net");
			//		//					c.OAuth2("oauth2")
			//		//						.Description("Seguridad OAuth2")
			//		//						.Flow("implicit")
			//		//						//	.AuthorizationUrl("https://localhost:44333/core/connect/authorize")
			//		//#if TEST
			//		//						.TokenUrl("http://payin-test.cloudapp.net/token")
			//		//#elif HOMO
			//		//						.TokenUrl("http://payin-homo.cloudapp.net/token")
			//		//#elif PRODUCTION
			//		//						.TokenUrl("https://control.pay-in.es/token")
			//		//#else // DEBUG
			//		//						.TokenUrl("http://localhost:8080/token")
			//		//#endif
			//		//	.Scopes(scopes =>
			//		//	{
			//		//		scopes.Add("read", "Read access to protected resources");
			//		//		scopes.Add("write", "Write access to protected resources");
			//		//	});
			//		//c
			//		//	.OperationFilter(new ApplyActionXmlComments(xmlCommentsPath))
			//		//	.ModelFilter(new ApplyTypeXmlComments(xmlCommentsPath));

			//		c.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + @"\Content\PayIn.DistributedServices.Payments.XML");
			//		c.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + @"\Content\PayIn.DistributedServices.Transport.XML");
			//		c.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + @"\Content\PayIn.DistributedServices.SmartCity.XML");
			//		c.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + @"\Content\PayIn.DistributedServices.XML");
			//	})
			//	.EnableSwaggerUi( c => {
			//		//c.EnableOAuth2Support("PayInPaymentApi", "PayInPayment@1912", "", "Pay[in] Api");
			//	});
			
			app
				.UseSecurity("token", config)
				.UseWebApi_JustMoney("justmoney/api", config)
			;

			// Temporal SATEL
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
