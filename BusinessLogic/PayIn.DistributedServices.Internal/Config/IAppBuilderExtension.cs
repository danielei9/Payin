using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PayIn.Common.DI.Internal;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.Formatters;
using Xp.DistributedServices.ModelBinder;

namespace Owin
{
	public static class IAppBuilderExtension
	{
		#region UseWebApi
		public static IAppBuilder UseWebApi(this IAppBuilder app, string path, HttpConfiguration config)
		{
			Initialize(config);
			RegisterRoutes(config, path);

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
			//			config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());

			// Filters
			config.Filters.Add(new XpErrorFilterAttribute());
			config.Filters.Add(new ValidateModelFilterAttribute());
			config.Filters.Add(new CheckModelForNullAttribute());
			//config.Filters.Add(new XpAuthorizeAttribute());
			//config.Filters.Add(new RequireHttpsAttribute());
			//config.Filters.Add(new ActionVerbRequiredAttribute());

			// Dependency injection
			DIConfig.InitializeWebApi(config);
		}
		#endregion Initialize

		#region RegisterRoutes
		public static void RegisterRoutes(HttpConfiguration config, string path)
		{
			config.MapHttpAttributeRoutes();
			//return;

			//#region PUT
			//config.Routes.MapHttpRoute(
			//	"PutHttpRoute",
			//	path + "/{controller}/{id}",
			//	defaults: new
			//	{
			//		id = RouteParameter.Optional,
			//		action = "put",
			//	},
			//	constraints: new
			//	{
			//		id = @"\d+",
			//		httpMethod = new HttpMethodConstraint(HttpMethod.Put)
			//	}
			//);
			//#endregion PUT

			//#region GET
			//config.Routes.MapHttpRoute(
			//	"GetHttpRoute",
			//	path + "/{controller}",
			//	defaults: new
			//	{
			//		action = "get"
			//	},
			//	constraints: new
			//	{
			//		httpMethod = new HttpMethodConstraint(HttpMethod.Get)
			//	}
			//);
			//config.Routes.MapHttpRoute(
			//	"GetIdHttpRoute",
			//	path + "/{controller}/{id}",
			//	defaults: new
			//	{
			//		id = RouteParameter.Optional,
			//		action = "get"
			//	},
			//	constraints: new
			//	{
			//		id = @"\d+",
			//		httpMethod = new HttpMethodConstraint(HttpMethod.Get)
			//	}
			//);
			//config.Routes.MapHttpRoute(
			//	"GetIdActionHttpRoute",
			//	path + "/{controller}/{id}/{action}",
			//	defaults: new
			//	{
			//		id = RouteParameter.Optional,
			//		action = RouteParameter.Optional
			//	},
			//	constraints: new
			//	{
			//		id = @"\d+",
			//		httpMethod = new HttpMethodConstraint(HttpMethod.Get)
			//	}
			//);
			//config.Routes.MapHttpRoute(
			//	"GetActionParamHttpRoute",
			//	path + "/{controller}/{action}/{param}",
			//	defaults: new
			//	{
			//		action = RouteParameter.Optional,
			//		param = RouteParameter.Optional
			//	},
			//	constraints: new
			//	{
			//		httpMethod = new HttpMethodConstraint(HttpMethod.Get)
			//	}
			//);
			//#endregion GET

			//#region POST
			//config.Routes.MapHttpRoute(
			//	"PostHttpRoute",
			//	path + "/{controller}",
			//	defaults: new
			//	{
			//		action = "post",
			//	},
			//	constraints: new
			//	{
			//		httpMethod = new HttpMethodConstraint(HttpMethod.Post)
			//	}
			//);
			//#endregion POST

			//#region DELETE
			//config.Routes.MapHttpRoute(
			//	"DeleteHttpRoute",
			//	path + "/{controller}/{id}",
			//	defaults: new
			//	{
			//		action = "delete",
			//	},
			//	constraints: new
			//	{
			//		id = @"\d+",
			//		httpMethod = new HttpMethodConstraint(HttpMethod.Delete)
			//	}
			//);
			//#endregion DELETE

			//config.Routes.MapHttpRoute(
			//	"DefaultHttpIdRoute",
			//	path + "/{controller}/{id}/{action}",
			//	defaults: new
			//	{
			//		action = RouteParameter.Optional,
			//	},
			//	constraints: new
			//	{
			//		id = @"\d+",
			//	}
			//);

			//config.Routes.MapHttpRoute(
			//	"DefaultHttpRoute",
			//	path + "/{controller}/{action}",
			//	defaults: new
			//	{
			//		action = RouteParameter.Optional,
			//	}
			//);

		}
		#endregion RegisterRoutes
	}
}
