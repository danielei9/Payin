using PayIn.Application.Dto.Arguments.ControlTrack;
using PayIn.Application.Dto.Results.ControlTrack;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/ControlTrack")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Operator
	)]
	public class ControlTrackController : ApiController
	{
		#region GET /{filter?}
		[HttpGet]
		[Route("{filter?}")]
		public async Task<ResultBase<ControlTrackGetAllResult>> Get(
			string filter,
			[FromUri] ControlTrackGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ControlTrackGetAllArguments, ControlTrackGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{filter?}

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ControlTrackGetResult>> GetItem(
			[FromUri] ControlTrackGetArguments arguments,
			[Injection] IQueryBaseHandler<ControlTrackGetArguments, ControlTrackGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /Item
		[HttpGet]
		[Route("Item")]
		public async Task<ResultBase<ControlTrackGetItemResult>> GetItem(
			[FromUri] ControlTrackGetItemArguments arguments,
			[Injection] IQueryBaseHandler<ControlTrackGetItemArguments, ControlTrackGetItemResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Item
	}
}
