using PayIn.Application.Dto.Arguments.ControlFormAssign;
using PayIn.Application.Dto.Results.ControlFormAssign;
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
	[RoutePrefix("Api/ControlFormAssign")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Operator
	)]
	public class ControlFormAssignController : ApiController
	{
		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ControlFormAssignGetResult>> Get(
			[FromUri] ControlFormAssignGetArguments arguments,
			[Injection] IQueryBaseHandler<ControlFormAssignGetArguments, ControlFormAssignGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /{filter?}
		[HttpGet]
		[Route("{filter?}")]
		public async Task<ResultBase<ControlFormAssignGetCheckResult>> GetCheck(
			[FromUri] ControlFormAssignGetCheckArguments arguments,
			[Injection] IQueryBaseHandler<ControlFormAssignGetCheckArguments, ControlFormAssignGetCheckResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{filter?}

		#region GET /Ids/{ids}
		[HttpGet]
		[Route("Ids/{ids}")]
		public async Task<ResultBase<ControlFormAssignGetIdsResult>> GetIds(
			[FromUri] ControlFormAssignGetIdsArguments arguments,
			[Injection] IQueryBaseHandler<ControlFormAssignGetIdsArguments, ControlFormAssignGetIdsResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Ids/{ids}

		#region POST /
		[HttpPost]
		[Route("")]
		public async Task<dynamic> Post(
			ControlFormAssignCreateArguments command,
			[Injection] IServiceBaseHandler<ControlFormAssignCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion POST /

		#region PUT /{ids}
		[HttpPut]
		[Route("{ids}")]
		public async Task<dynamic> Put(
			ControlFormAssignUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ControlFormAssignUpdateArguments> handler
		)
		{
			await handler.ExecuteAsync(arguments);
			return null;
		}
		#endregion PUT /{ids}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] ControlFormAssignDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ControlFormAssignDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}
	}
}
