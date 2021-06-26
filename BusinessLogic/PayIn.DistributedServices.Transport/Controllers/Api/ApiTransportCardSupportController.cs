using PayIn.Application.Dto.Transport.Arguments.TransportCardSupport;
using PayIn.Application.Dto.Transport.Results.TransportCardSupport;
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
	[RoutePrefix("Api/TransportCardSupport")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.TransportOperator
	)]
	public class ApiTransportCardSupportController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<TransportCardSupportGetAllResult>> GetAll(
			[FromUri] TransportCardSupportGetAllArguments arguments,
			[Injection] IQueryBaseHandler<TransportCardSupportGetAllArguments, TransportCardSupportGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<TransportCardSupportGetResult>> Get(
			[FromUri] TransportCardSupportGetArguments arguments,
			[Injection] IQueryBaseHandler<TransportCardSupportGetArguments, TransportCardSupportGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /Selector/{filter?}
		[HttpGet]
		[Route("Selector/{filter?}")]
		public async Task<ResultBase<TransportCardSupportGetSelectorResult>> Selector(
			string filter,
			[FromUri] TransportCardSupportGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<TransportCardSupportGetSelectorArguments, TransportCardSupportGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /Selector/{filter?}

        #region POST /
        [HttpPost]
		[Route]
		public async Task<dynamic> Post(
			TransportCardSupportCreateArguments arguments,
			[Injection] IServiceBaseHandler<TransportCardSupportCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion POST /

		#region PUT /
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
			TransportCardSupportUpdateArguments arguments,
			[Injection] IServiceBaseHandler<TransportCardSupportUpdateArguments> handler
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
			[FromUri] TransportCardSupportDeleteArguments arguments,
			[Injection] IServiceBaseHandler<TransportCardSupportDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /
	}
}
