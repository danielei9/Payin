using PayIn.Application.Dto.Transport.Arguments.TransportCardApplication;
using PayIn.Application.Dto.Transport.Results.TransportCardApplication;
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
	[RoutePrefix("Api/TransportCardApplication")]
	[XpAuthorize(
	ClientIds = AccountClientId.Web,
	Roles = AccountRoles.Superadministrator + "," + AccountRoles.TransportOperator)]
	public class ApiTransportCardApplicationController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<TransportCardApplicationGetAllResult>> GetAll(
			[FromUri] TransportCardApplicationGetAllArguments arguments,
			[Injection] IQueryBaseHandler<TransportCardApplicationGetAllArguments, TransportCardApplicationGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		//#region GET /{id:int}
		//[HttpGet]
		//[Route("{id:int}")]
		//public async Task<ResultBase<TransportCardApplicationGetAllResult>> Get(
		//	[FromUri] TransportCardApplicationGetArguments arguments,
		//	[Injection] IQueryBaseHandler<TransportCardApplicationGetArguments, TransportCardApplicationGetAllResult> handler
		//)
		//{
		//	var result = await handler.ExecuteAsync(arguments);
		//	return result;
		//}
		//#endregion GET /{id:int}
	}
}
