using PayIn.Application.Dto.Arguments.ControlPlanningCheck;
using PayIn.Application.Dto.Results.ControlPlanningCheck;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
	[HideSwagger]
	[RoutePrefix("Mobile/ControlPlanningCheck")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User + "," + AccountRoles.Commerce
	)]
	public class MobileControlPlanningCheckController : ApiController
	{
		#region GET /v1
		[HttpGet]
		[Route("v1")]
		public async Task<ResultBase<ControlPlanningCheckMobileGetResult>> Get(
			[FromUri] ControlPlanningCheckMobileGetArguments command,
			[Injection] IQueryBaseHandler<ControlPlanningCheckMobileGetArguments, ControlPlanningCheckMobileGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /v1
	}
}
