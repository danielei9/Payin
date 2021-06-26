using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PayIn.Common.DI.JustMoney;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.Formatters;
using Xp.DistributedServices.ModelBinder;

namespace Owin
{
	public static class IAppBuilderExtension
	{
		#region UseWebApi_JustMoney
		public static IAppBuilder UseWebApi_JustMoney(this IAppBuilder app, string path, HttpConfiguration config)
		{
			Initialize(config);
			RegisterRoutes(config, path);
			
			app
				.UseWebApi(config)
				;

			return app;
		}
		#endregion UseWebApi_JustMoney

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

			// http://www.binaryintellect.net/articles/20e86f97-9f59-431d-9e4b-7f248e7a9511.aspx
			// By default ASP.NET Web API uses DataContractSerializer to serialize XML data.
			// That's why those XML namespaces become necessary in your markup.
			// You can skip that if you configure Web API serialization to use XmlSerializer.
			config.Formatters.XmlFormatter.UseXmlSerializer = true;

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

			// Dependency injection
			DIConfig.InitializeWebApi(config);
			//DIConfig.RegisterTypePerResolve<IPushAndroidService, PushAndroidService>();
			//DIConfig.RegisterTypePerResolve<IPushSignalRService, PushSignalRService>();
			//DIConfig.RegisterTypePerResolve<IInverseConnection, InverseConnection>();

			// SignalR camel case
			//var settings = new JsonSerializerSettings();
			//settings.ContractResolver = new SignalRContractResolver();
			//var serializer = JsonSerializer.Create(settings);
			//GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
		}
		#endregion Initialize

		#region RegisterRoutes
		public static void RegisterRoutes(HttpConfiguration config, string path)
		{
			config.MapHttpAttributeRoutes();
		}
		#endregion RegisterRoutes
	}
}
