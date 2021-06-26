using PayIn.Application.Dto.Transport.Arguments.TransportCard;
using PayIn.Application.Dto.Transport.Results.TransportCard;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Transport.Controllers
{
	[HideSwagger]
	[RoutePrefix("Mobile/TransportCard")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class MobileTransportCardController : ApiController
	{
		/*#region POST /v1/SmartBand
		[HttpPost]
		[Route("v1/SmartBand")]
		public async Task<dynamic> Post(
				TransportCardCreateArguments command,
			[Injection] IServiceBaseHandler<TransportCardCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion POST /v1/SmartBand*/

		/*#region GET /v1/SmartBand
		[HttpGet]
		[Route("v1/SmartBand")]
		public async Task<ResultBase<TransportCardGetResult>> Get(
			[FromUri] TransportCardGetArguments arguments,
			[Injection] IQueryBaseHandler<TransportCardGetArguments, TransportCardGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/SmartBand*/

		#region GET /v1/GetByDeviceAddress
		[HttpGet]
		[Route("v1/GetByDeviceAddress")]
		public async Task<ResultBase<TransportCardGetByDeviceAddressResult>> Get(
			[FromUri] TransportCardGetByDeviceAddressArguments arguments,
			[Injection] IQueryBaseHandler<TransportCardGetByDeviceAddressArguments, TransportCardGetByDeviceAddressResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/GetByDeviceAddress

		#region Delete /{id:int}
		[HttpDelete]
		[Route("v1/{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] TransportCardDeleteArguments arguments,
			[Injection] IServiceBaseHandler<TransportCardDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return new { Id = result.Id  };
		}
		#endregion Delete /{id:int}
	}
}
