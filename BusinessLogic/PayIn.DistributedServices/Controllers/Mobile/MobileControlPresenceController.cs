using PayIn.Application.Dto.Arguments.ControlPresence;
using PayIn.Application.Dto.Results.ControlPresence;
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
	[RoutePrefix("Mobile/ControlPresence")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class MobileControlPresenceController : ApiController
	{
		#region GET /v1/TimeTable
		[HttpGet]
		[Route("v1/Timetable")]
		public async Task<ResultBase<ControlPresenceMobileGetTimetableResult>> GetTimetable(
			[FromUri] ControlPresenceMobileGetTimetableArguments arguments,
			[Injection] IQueryBaseHandler<ControlPresenceMobileGetTimetableArguments, ControlPresenceMobileGetTimetableResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/Timetable

		#region GET /v1/Tag
		[HttpGet]
		[Route("v1/Tag")]
		public async Task<ResultBase<ControlPresenceMobileGetTagResult>> GetTag(
			[FromUri] ControlPresenceMobileGetTagArguments arguments,
			[Injection] IQueryBaseHandler<ControlPresenceMobileGetTagArguments, ControlPresenceMobileGetTagResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/Tag

		#region POST /v1/Check
		[HttpPost]
		[Route("v1/Check")]
		public async Task<dynamic> Check(
			ControlPresenceMobileCheckArguments arguments,
			[Injection] IServiceBaseHandler<ControlPresenceMobileCheckArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v1/Check

		#region POST /v1/Track
		[HttpPost]
		[Route("v1/Track")]
		public async Task Track(
			ControlPresenceMobileTrackArguments arguments,
			[Injection] IServiceBaseHandler<ControlPresenceMobileTrackArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
		}
		#endregion POST /v1/Track
	}
}
