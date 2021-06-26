using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Application.Dto.SmartCity.Results;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.SmartCity.Controllers.Sentilo
{
	[HideSwagger]
	[RoutePrefix("Sentilo/Data")]
    //[XpAuthorize(
    //       ClientIds = AccountClientId.Web,
    //       Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
    //)]
    public class SentiloDataController : ApiController
	{
		//#region DELETE /{providerId:int}
		//[HttpDelete]
		//[Route("{providerId:int}")]
		//public async Task<dynamic> DeleteSensors(
		//	int providerId,
		//	[Injection] IServiceBaseHandler<PublicDataDeleteSensorsArguments> handler
		//)
		//{
		//	var arguments = new PublicDataDeleteSensorsArguments(providerId);

		//	var item = await handler.ExecuteAsync(arguments);
		//	return new { item.Id };
		//}
		//#endregion DELETE /{providerId:int}

		//#region DELETE /{providerId:int}/{sensorId:int}
		//[HttpDelete]
		//[Route("{providerId:int}/{sensorId:int}")]
		//public async Task<dynamic> DeleteSensor(
		//	int providerId,
		//	int sensorId,
		//	[Injection] IServiceBaseHandler<PublicDataDeleteSensorArguments> handler
		//)
		//{
		//	var arguments = new PublicDataDeleteSensorArguments(providerId, sensorId);

		//	var item = await handler.ExecuteAsync(arguments);
		//	return new { item.Id };
		//}
		//#endregion DELETE /{providerId:int}/{sensorId:int}

		//#region GET /{providerId:int}
		//[HttpGet]
		//[Route("{providerId:int}")]
		//public async Task<SentiloDataGetByProviderResult> GetByProvicer(
		//	int providerId,
		//	[FromUri] SentiloDataGetByProviderArguments arguments,
		//	[Injection] IQueryBaseHandler<SentiloDataGetByProviderArguments, SentiloDataGetByProviderResult> handler
		//)
		//{
		//	arguments.ProviderId = providerId;

		//	var items = await handler.ExecuteAsync(arguments);
		//	return items.Data.FirstOrDefault();
		//}
		//#endregion GET /{providerId:int}

		//#region GET /{providerId:int}/{sensorId:int}
		//[HttpGet]
		//[Route("{providerId:int}/{sensorId:int}")]
		//public async Task<SentiloDataGetBySensorResult> GetBySensor(
		//	int providerId,
		//	int sensorId,
		//	[FromUri] SentiloDataGetBySensorArguments arguments,
		//	[Injection] IQueryBaseHandler<SentiloDataGetBySensorArguments, SentiloDataGetBySensorResult> handler
		//)
		//{
		//	arguments.ProviderId = providerId;
		//	arguments.SensorId = sensorId;

		//	var items = await handler.ExecuteAsync(arguments);
		//	return items.Data.FirstOrDefault();
		//}
		//#endregion GET /{providerId:int}/{sensorId:int}

		#region PUT /{providerCode}
		[HttpPut]
		[Route("{providerCode}")]
		public async Task PutProvider(
			string providerCode,
			[FromBody] SentiloDataUpdateProviderArguments arguments,
			[Injection] IServiceBaseHandler<SentiloDataUpdateProviderArguments> handler
		)
		{
			arguments.ProviderCode = providerCode;

			await handler.ExecuteAsync(arguments);
		}
		#endregion PUT /{providerCode}

		#region PUT /{providerCode}/{sensorCode}
		[HttpPut]
		[Route("{providerCode}/{sensorCode}")]
		public async Task PutSensor(
			string providerCode,
			string sensorCode,
			[FromBody] SentiloDataUpdateSensorArguments arguments,
			[Injection] IServiceBaseHandler<SentiloDataUpdateSensorArguments> handler
		)
		{
			arguments.ProviderCode = providerCode;
			arguments.SensorCode = sensorCode;

			await handler.ExecuteAsync(arguments);
		}
		#endregion PUT /{providerCode}/{sensorCode}

		#region PUT /{providerCode}/{sensorCode}/{value:decimal}
		[HttpPut]
		[Route("{providerCode}/{sensorCode}/{value:decimal}")]
		public async Task<dynamic> Put(
			string providerCode,
			string sensorCode,
			decimal value,
			[Injection] IServiceBaseHandler<SentiloDataUpdateValueArguments> handler
		)
		{
			var arguments = new SentiloDataUpdateValueArguments(providerCode, sensorCode, value);
			await handler.ExecuteAsync(arguments);

			return null;
		}
		#endregion PUT /{providerCode}/{sensorCode}/{value:decimal}
	}
}
