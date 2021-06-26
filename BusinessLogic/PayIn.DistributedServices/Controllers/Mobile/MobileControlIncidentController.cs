using PayIn.Application.Dto.Arguments.ControlIncident;
using PayIn.Application.Dto.Results.ControlIncident;
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
	[RoutePrefix("Mobile/ControlIncident")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class MobileControlIncidentController : ApiController
	{
		/* Deprecated */
		#region GET /v1/Items
		[HttpGet]
		[Route("v1/Items")]
		public async Task<ResultBase<ControlIncidentGetItemsResult>> Items(
			[FromUri] ControlIncidentGetItemsArguments arguments,
			[Injection] IQueryBaseHandler<ControlIncidentGetItemsArguments, ControlIncidentGetItemsResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/Timetable

		/* Deprecated */
		#region POST /v1/
		[HttpPost]
		[Route("v1/")]
		public async Task<dynamic> Post(
			ControlIncidentCreateArguments arguments,
			[Injection] IServiceBaseHandler<ControlIncidentCreateArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v1/

        #region POST /v1/ManualCheck
        [HttpPost]
		[Route("v1/ManualCheck/")]
		public async Task<dynamic> Post(
			ControlIncidentCreateManualCheckArguments arguments,
			[Injection] IServiceBaseHandler<ControlIncidentCreateManualCheckArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
        }
        #endregion POST /v1/ManualCheck
    }
}
