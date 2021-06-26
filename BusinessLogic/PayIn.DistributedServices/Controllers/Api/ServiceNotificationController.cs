using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Results;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Mobile
{
	[HideSwagger]
	[RoutePrefix("Api/ServiceNotification")]  
	//[XpAuthorize(
	//	ClientIds = AccountClientId.Web,
	//	Roles = AccountRoles.User
	//)]
	public class ServiceNotificationController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		public async Task<ResultBase<ServiceNotificationGetAllResult>> GetAll(
			[FromUri] ServiceNotificationGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ServiceNotificationGetAllArguments, ServiceNotificationGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /


		#region POST
		[HttpPost]
		[Route]
		public async Task<dynamic> Post(
			ServiceNotificationCreateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceNotificationCreateArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST
	}
}
