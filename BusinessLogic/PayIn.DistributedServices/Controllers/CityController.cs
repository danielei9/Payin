using PayIn.Application.Dto.Arguments.ServiceCity;
using PayIn.Application.Dto.Results.ServiceCity;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
	public class CityController : ApiController
	{
		#region GET /ConcessionType
		[HttpGet]
		public async Task<ResultBase<ServiceCityGetByZoneTypeResult>> ConcessionType(
			[FromUri] ServiceCityGetByZoneTypeArguments command,
			[Injection] IQueryBaseHandler<ServiceCityGetByZoneTypeArguments, ServiceCityGetByZoneTypeResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /

		#region GET /Selector
		[HttpGet]
		public async Task<ResultBase<ServiceCityGetSelectorResult>> Selector(
			[FromUri] ServiceCityGetSelectorArguments command,
			[Injection] IQueryBaseHandler<ServiceCityGetSelectorArguments, ServiceCityGetSelectorResult> handler
		)
		{
						var result = await handler.ExecuteAsync(command);
						return result;
		}
		#endregion GET /Selector
	}
}
