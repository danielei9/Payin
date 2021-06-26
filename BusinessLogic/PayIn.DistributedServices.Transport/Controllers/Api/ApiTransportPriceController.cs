using PayIn.Application.Dto.Transport.Arguments.TransportPrice;
using PayIn.Application.Dto.Transport.Results.TransportPrice;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Transport.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/TransportPrice")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.TransportOperator
	)]
	public class ApiTransportPriceController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("{titleId?}")]
		public async Task<ResultBase<TransportPriceGetAllResult>> GetAll(
			[FromUri] TransportPriceGetAllArguments arguments,
			[Injection] IQueryBaseHandler<TransportPriceGetAllArguments, TransportPriceGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<TransportPriceGetResult>> Get(
			[FromUri] TransportPriceGetArguments arguments,
			[Injection] IQueryBaseHandler<TransportPriceGetArguments, TransportPriceGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region POST /
		[HttpPost]
		[Route]
		public async Task<dynamic> Post(
			TransportPriceCreateArguments arguments,
			[Injection] IServiceBaseHandler<TransportPriceCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id }; ;
		}
		#endregion POST /

		#region PUT /
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
			TransportPriceUpdateArguments arguments,
			[Injection] IServiceBaseHandler<TransportPriceUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /

		#region DELETE /
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] TransportPriceDeleteArguments arguments,
			[Injection] IServiceBaseHandler<TransportPriceDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /

	}
}
