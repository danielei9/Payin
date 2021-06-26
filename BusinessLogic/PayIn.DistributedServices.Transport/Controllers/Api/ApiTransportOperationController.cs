using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Dto.Transport.Results.TransportOperation;
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
	[RoutePrefix("Api/Transport")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles =  AccountRoles.Superadministrator
	)]
	public class ApiTransportOperationController : ApiController
	{
		#region GET /Transport
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<TransportOperationGetAllResult>> GetAll(
			[FromUri] TransportOperationGetAllArguments arguments,
			[Injection] IQueryBaseHandler<TransportOperationGetAllArguments, TransportOperationGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Transport

		#region GET /Details/{id:long}
		[HttpGet]
		[Route("Details/{id:long}")]
		public async Task<ResultBase<TransportOperationGetDetailsResult>> GetAll(
			[FromUri] TransportOperationGetDetailsArguments arguments,
			[Injection] IQueryBaseHandler<TransportOperationGetDetailsArguments, TransportOperationGetDetailsResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Details/{id:long}

		#region GET /Details/{id:long}/{uid:long}
		[HttpGet]
		[Route("Details/{id:long}/{uid:long}")]
		public async Task<ResultBase<TransportOperationGetDetailsResult>> GetAll2(
			[FromUri] TransportOperationGetDetailsArguments arguments,
			[Injection] IQueryBaseHandler<TransportOperationGetDetailsArguments, TransportOperationGetDetailsResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Details/{id:long}/{uid:long}

			#region POST /AddBlackList
		[HttpPost]
		[Route("AddBlackList")]
		public async Task<dynamic> AddBlackList(
			TransportOperationAddBlackListArguments arguments,
			[Injection] IServiceBaseHandler<TransportOperationAddBlackListArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /AddBlackList
	}
}
