using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PayIn.Common.DI.Public;
using PayIn.DistributedServices.RealTime;
using PayIn.DistributedServices.Tsm;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using System.Web.Http.Routing;
using Xp.Application;
using Xp.DistributedServices;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.Formatters;
using Xp.DistributedServices.ModelBinder;
using Xp.Infrastructure.Services;

namespace Owin
{
	public static class IAppBuilderExtension
	{
		#region UseWebApi
		public static IAppBuilder UseWebApi(this IAppBuilder app, string path, HttpConfiguration config)
		{
			Initialize(config);
			RegisterRoutes(config, path);

			//app
			//	.MapSignalR()
			app
				//.MapSignalR<TsmHub>("/Connections/DemoPersistentConnection")
				.MapSignalR("/signalr", new HubConfiguration() {
					EnableDetailedErrors = true,
					EnableJavaScriptProxies = false
				})
				//.Map("/signalr", map =>
				//{
				//	Setup the CORS middleware to run before SignalR.
				//	 By default this will allow all origins. You can
				//	 configure the set of origins and/ or http verbs by
				//	  providing a cors options with a different policy.
				//	 map.UseCors(CorsOptions.AllowAll);
				//	var hubConfiguration = new HubConfiguration
				//	{
				//		 You can enable JSONP by uncommenting line below.
				//		 JSONP requests are insecure but some older browsers (and some
				//		 versions of IE) require JSONP to work cross domain
				//		 EnableJSONP = true
				//	};
				//	Run the SignalR pipeline. We're not using MapSignalR
				//	 since this branch already runs under the "/signalr"
				//	 path.
				//	map.RunSignalR(hubConfiguration);
				//})
				.UseWebApi(config)
				;

			return app;
		}
		#endregion UseWebApi

		#region Initialize
		public static void Initialize(HttpConfiguration config)
		{
			// Formatters
			config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				NullValueHandling = NullValueHandling.Ignore,
				Formatting = Formatting.None,
			};
			config.Formatters.Remove(config.Formatters.XmlFormatter);
			config.Formatters.Add(new CsvFormatter());
			config.Formatters.Add(new MultiFormDataMediaTypeFormatter());
			config.Formatters.Insert(0, new BinaryMediaTypeFormatter());

			// Model binder
			config.Services.Insert(typeof(ModelBinderProvider), 0, new CommandModelBinderProvider());

			//#region DEBUG
			config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
			//#endregion DEBUG

			// Filters
			config.Filters.Add(new XpErrorFilterAttribute());
			config.Filters.Add(new ValidateModelFilterAttribute());
			config.Filters.Add(new CheckModelForNullAttribute());
			//config.Filters.Add(new XpAuthorizeAttribute());
			//config.Filters.Add(new RequireHttpsAttribute());
			//config.Filters.Add(new ActionVerbRequiredAttribute());

			// Dependency injection
			DIConfig.InitializeWebApi(config);
			DIConfig.RegisterTypePerResolve<IPushAndroidService, PushAndroidService>();
			DIConfig.RegisterTypePerResolve<IPushExpoService, PushExpoService>();
			DIConfig.RegisterTypePerResolve<IPushSignalRService, PushSignalRService>();
			DIConfig.RegisterTypePerResolve<IInverseConnection, InverseConnection>();

			// SignalR camel case
			var settings = new JsonSerializerSettings();
			settings.ContractResolver = new SignalRContractResolver();
			var serializer = JsonSerializer.Create(settings);
			GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
		}
		#endregion Initialize

		#region RegisterRoutes
		public static void RegisterRoutes(HttpConfiguration config, string path)
		{
			config.MapHttpAttributeRoutes();

			#region PUT
			config.Routes.MapHttpRoute(
				"PutHttpRoute",
				path + "/{controller}/{id}",
				defaults: new
				{
					id = RouteParameter.Optional,
					action = "put",
				},
				constraints: new
				{
					id = @"\d+",
					httpMethod = new HttpMethodConstraint(HttpMethod.Put)
				}
			);
			#endregion PUT

			#region GET
			config.Routes.MapHttpRoute(
				"GetHttpRoute",
				path + "/{controller}",
				defaults: new
				{
					action = "get"
				},
				constraints: new
				{
					httpMethod = new HttpMethodConstraint(HttpMethod.Get)
				}
			);
			config.Routes.MapHttpRoute(
				"GetIdHttpRoute",
				path + "/{controller}/{id}",
				defaults: new
				{
					id = RouteParameter.Optional,
					action = "get"
				},
				constraints: new
				{
					id = @"\d+",
					httpMethod = new HttpMethodConstraint(HttpMethod.Get)
				}
			);
			config.Routes.MapHttpRoute(
				"GetIdActionHttpRoute",
				path + "/{controller}/{id}/{action}",
				defaults: new
				{
					id = RouteParameter.Optional,
					action = RouteParameter.Optional
				},
				constraints: new
				{
					id = @"\d+",
					httpMethod = new HttpMethodConstraint(HttpMethod.Get)
				}
			);
			config.Routes.MapHttpRoute(
				"GetActionParamHttpRoute",
				path + "/{controller}/{action}/{param}",
				defaults: new
				{
					action = RouteParameter.Optional,
					param = RouteParameter.Optional
				},
				constraints: new
				{
					httpMethod = new HttpMethodConstraint(HttpMethod.Get)
				}
			);
			#endregion GET

			#region POST
			config.Routes.MapHttpRoute(
				"PostHttpRoute",
				path + "/{controller}",
				defaults: new
				{
					action = "post",
				},
				constraints: new
				{
					httpMethod = new HttpMethodConstraint(HttpMethod.Post)
				}
			);
			#endregion POST

			#region DELETE
			config.Routes.MapHttpRoute(
				"DeleteHttpRoute",
				path + "/{controller}/{id}",
				defaults: new
				{
					action = "delete",
				},
				constraints: new
				{
					id = @"\d+",
					httpMethod = new HttpMethodConstraint(HttpMethod.Delete)
				}
			);
			#endregion DELETE

			config.Routes.MapHttpRoute(
				"DefaultHttpIdRoute",
				path + "/{controller}/{id}/{action}",
				defaults: new
				{
					action = RouteParameter.Optional,
				},
				constraints: new
				{
					id = @"\d+",
				}
			);

			config.Routes.MapHttpRoute(
				"DefaultHttpRoute",
				path + "/{controller}/{action}",
				defaults: new
				{
					action = RouteParameter.Optional,
				}
			);

		}
		#endregion RegisterRoutes
	}
}
