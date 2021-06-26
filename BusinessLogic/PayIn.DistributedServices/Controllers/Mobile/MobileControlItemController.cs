using PayIn.Application.Dto.Arguments.ControlItem;
using PayIn.Application.Dto.Results.ControlItem;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Mobile
{
	[HideSwagger]
	[RoutePrefix("Mobile/ControlItem")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class MobileControlItemController : ApiController
	{
		#region GET /v1
		[HttpGet]
		[Route("v1")]
		public async Task<ResultBase<ControlItemMobileGetAllResult>> Items(
			[FromUri] ControlItemMobileGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ControlItemMobileGetAllArguments, ControlItemMobileGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1

		#region GET /v1/Selector/{filter}
		[HttpGet]
		[Route("v1/Selector/{filter?}")]
		public async Task<ResultBase<ControlItemMobileGetSelectorResult>> Selector(
			string filter,
			[FromUri] ControlItemMobileGetSelectorArguments command,
			[Injection] IQueryBaseHandler<ControlItemMobileGetSelectorArguments, ControlItemMobileGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /v1/Selector/{filter}
	}
}
