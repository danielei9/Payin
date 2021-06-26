using PayIn.Application.Dto.Arguments.ControlTrack;
using PayIn.Application.Dto.Results.ControlTrack;
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
	[RoutePrefix("Public/ControlTrack")]
	[XpAuthorize(
		ClientIds = AccountClientId.Api,
		Roles = AccountRoles.ControlApi
	)]
	public class PublicControlTrackController : ApiController
	{
		#region GET /v1/Day
		[HttpGet]
		[Route("v1/Day")]
		public async Task<ResultBase<ControlTrackPublicGetByDayResult>> GetTimetable(
			[FromUri] ControlTrackPublicGetByDayArguments arguments,
			[Injection] IQueryBaseHandler<ControlTrackPublicGetByDayArguments, ControlTrackPublicGetByDayResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/Day

		#region GET /v1/Range
		[HttpGet]
		[Route("v1/Range")]
		public async Task<ResultBase<ControlTrackPublicGetByRangeResult>> GetTimetable(
			[FromUri] ControlTrackPublicGetByRangeArguments arguments,
			[Injection] IQueryBaseHandler<ControlTrackPublicGetByRangeArguments, ControlTrackPublicGetByRangeResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/Range
	}
}
