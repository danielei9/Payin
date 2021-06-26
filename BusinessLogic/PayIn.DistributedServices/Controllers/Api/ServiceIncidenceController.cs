using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Results;
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
	[RoutePrefix("Api/ServiceIncidence")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.Web,
		Roles = AccountRoles.User // AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker + "," + AccountRoles.User
	)]
	public class ServiceIncidenceController : ApiController
	{
		#region GET /{filter?}
		[HttpGet]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.Web//,
			//Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.User
		)]
		public async Task<ResultBase<ServiceIncidenceGetAllResult>> GetAll(
			[FromUri] ServiceIncidenceGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ServiceIncidenceGetAllArguments, ServiceIncidenceGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{filter?}

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.Web,
			Roles = AccountRoles.User //AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.User
		)]
		public async Task<ResultBase<ServiceIncidenceGetResult>> Get(
			int id,
			[FromUri] ServiceIncidenceGetArguments arguments,
			[Injection] IQueryBaseHandler<ServiceIncidenceGetArguments, ServiceIncidenceGetResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}
		
		#region GET /Notifications/{id:int}
		[HttpGet]
		[Route("Notifications/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.Web,
			Roles = AccountRoles.User //AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.User
		)]
		public async Task<ResultBase<ServiceIncidenceGetResult_Notifications>> GetNotifications(
			int id,
			[FromUri] ServiceIncidenceGetArguments_Notifications arguments,
			[Injection] IQueryBaseHandler<ServiceIncidenceGetArguments_Notifications, ServiceIncidenceGetResult_Notifications> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Notifications/{id:int}

		#region POST /
		[HttpPost]
		[Route()]
		public async Task<dynamic> Post(
			ServiceIncidenceCreateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceIncidenceCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /

		#region PUT /
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
			int id,
			ServiceIncidenceUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceIncidenceUpdateArguments> handler
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /

		#region PUT /AddNotification
		[HttpPut]
		[Route("AddNotification/{id:int}")]
		public async Task<dynamic> PutAddNotification(
			int Id,
			MobileServiceNotificationCreateArguments arguments,
			[Injection] IServiceBaseHandler<MobileServiceNotificationCreateArguments> handler
		)
		{
			arguments.IncidenceId = Id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /AddNotification
	}
}
