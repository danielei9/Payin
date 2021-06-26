using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Application.Dto.SmartCity.Results;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.SmartCity.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("SmartCity/Reporting/Sensor")]
    //[XpAuthorize(
    //       ClientIds = AccountClientId.Web,
    //       Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
    //)]
    public class ReportingSensorController : ApiController
	{
		#region GET /PerDay/{id:int}
		[HttpGet]
		[Route("PerDay/{id:int}")]
		public async Task<ResultBase<ApiSensorGetPerDayResult>> GetPerHour(
			int id,
			[FromUri] ApiSensorGetPerDayArguments arguments,
			[Injection] IQueryBaseHandler<ApiSensorGetPerDayArguments, ApiSensorGetPerDayResult> handler
		)
		{
			arguments.Id = id;

			var items = await handler.ExecuteAsync(arguments);
			return items;
		}
		#endregion GET /PerHour/{id:int}
	}
}
