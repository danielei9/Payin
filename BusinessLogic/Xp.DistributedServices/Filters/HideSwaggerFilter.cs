using Swashbuckle.Swagger;
using System.Linq;
using System.Web.Http.Description;

namespace Xp.DistributedServices.Filters
{
	public class HideSwaggerFilter : IDocumentFilter
	{
		public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
		{
			foreach (var apiDescription in apiExplorer.ApiDescriptions)
			{
				if (
					!apiDescription.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<HideSwaggerAttribute>().Any() &&
					!apiDescription.ActionDescriptor.GetCustomAttributes<HideSwaggerAttribute>().Any()
				)
					continue;

				var route = "/" + apiDescription.Route.RouteTemplate.TrimEnd('/');
				swaggerDoc.paths.Remove(route);
			}
		}
	}
}
