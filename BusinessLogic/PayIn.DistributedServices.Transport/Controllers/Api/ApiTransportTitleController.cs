using PayIn.Application.Dto.Transport.Arguments.TransportTitle;
using PayIn.Application.Dto.Transport.Results.TransportTitle;
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
	[RoutePrefix("Api/TransportTitle")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.TransportOperator + "," + AccountRoles.Transport
	)]
	public class ApiTransportTitleController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<TransportTitleGetAllResult>> GetAll(
			[FromUri] TransportTitleGetAllArguments arguments,
			[Injection] IQueryBaseHandler<TransportTitleGetAllArguments, TransportTitleGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<TransportTitleGetResult>> Get(
			[FromUri] TransportTitleGetArguments arguments,
			[Injection] IQueryBaseHandler<TransportTitleGetArguments, TransportTitleGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /Selector/{filter?}
		[HttpGet]
		[Route("Selector/{filter?}")]
		public async Task<ResultBase<TransportTitleGetSelectorResult>> Selector(
			string filter,
			[FromUri] TransportTitleGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<TransportTitleGetSelectorArguments, TransportTitleGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Selector/{filter?}

		#region GET /Selector/TConcession/{filter?}
		[HttpGet]
		[Route("Selector/TConcessions/{filter?}")]
		public async Task<ResultBase<TransportTitleGetSelectorTConcessionResult>> Selector(
			string filter,
			[FromUri] TransportTitleGetSelectorTConcessionArguments arguments,
			[Injection] IQueryBaseHandler<TransportTitleGetSelectorTConcessionArguments, TransportTitleGetSelectorTConcessionResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Selector/TConcession/{filter?}

		#region POST /
		[HttpPost]
		[Route]
		public async Task<dynamic> Post(
			TransportTitleCreateArguments arguments,
			[Injection] IServiceBaseHandler<TransportTitleCreateArguments> handler
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
			TransportTitleUpdateArguments arguments,
			[Injection] IServiceBaseHandler<TransportTitleUpdateArguments> handler
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
			[FromUri] TransportTitleDeleteArguments arguments,
			[Injection] IServiceBaseHandler<TransportTitleDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /

		#region GET /SelectorTitle/{filter?}
		[HttpGet]
		[Route("SelectorTitle/{filter?}")]
		public async Task<ResultBase<PromotionGetTitleSelectorResult>> SelectorTitle(
			string filter,
			[FromUri] PromotionGetTitleSelectorArguments arguments,
			[Injection] IQueryBaseHandler<PromotionGetTitleSelectorArguments, PromotionGetTitleSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /SelectorTitle/{filter?}
	}
}
