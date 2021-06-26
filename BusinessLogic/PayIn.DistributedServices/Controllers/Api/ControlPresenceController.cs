using PayIn.Application.Dto.Arguments.ControlPresence;
using PayIn.Application.Dto.Results.ControlPresence;
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
	[RoutePrefix("Api/ControlPresence")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.User + "," + AccountRoles.Operator
	)]
	public class ControlPresenceController : ApiController
	{
		#region GET /Hours
		[HttpGet]
		[Route("Hours")]
		public async Task<ResultBase<ControlPresenceGetHoursResult>> GetHours(
			[FromUri] ControlPresenceGetHoursArguments arguments,
			[Injection] IQueryBaseHandler<ControlPresenceGetHoursArguments, ControlPresenceGetHoursResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Hours
	}
}
