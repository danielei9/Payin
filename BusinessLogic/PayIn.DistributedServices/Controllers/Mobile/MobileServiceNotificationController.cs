using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Arguments.ServiceNotification;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Results.ServiceNotification;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData.Query;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Mobile
{
	[HideSwagger]
	[RoutePrefix("Mobile/ServiceNotification")]
	public class MobileServiceNotificationController : ApiController
	{
		#region GET v1
		[HttpGet]
		[Route("v1")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.User
		)]
		public async Task<ResultBase<ServiceNotificationMobileGetAllResult>> GetAll(
			[FromUri] ServiceNotificationMobileGetAllArguments command,
			ODataQueryOptions<ServiceNotificationMobileGetAllResult> options,
            [Injection] IQueryBaseHandler<ServiceNotificationMobileGetAllArguments, ServiceNotificationMobileGetAllResult> handler
		)
		{
			command.Skip = options.Skip == null ? 0 : options.Skip.Value;
			command.Top = Math.Min(options.Top == null ? 100 : options.Top.Value, 100);

			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET v1

		#region POST v2
		[HttpPost]
		[Route("v2")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> PostV2(
			MobileServiceNotificationCreateArguments arguments,
			[Injection] IServiceBaseHandler<MobileServiceNotificationCreateArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return new { result.Id };
		}
		#endregion POST v2

		#region POST /v1/Photo/{id:int}
		[HttpPost]
		[Route("v1/Photo/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> Photo(
			int id,
			MobileServiceIncidenceCreatePhotoArguments arguments,
			[Injection] IServiceBaseHandler<MobileServiceIncidenceCreatePhotoArguments> handler)
		{
			arguments.IncidenceId = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /v1/Photo/{id:int}

		#region POST /v1/Position/{id:int}
		[HttpPost]
		[Route("v1/Position/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> Position(
			int id,
			MobileServiceIncidenceCreatePositionArguments arguments,
			[Injection] IServiceBaseHandler<MobileServiceIncidenceCreatePositionArguments> handler)
		{
			arguments.IncidenceId = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /v1/Position/{id:int}

		#region POST v1
		[HttpPost]
		[Route("v1")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> Post(
			ServiceNotificationCreateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceNotificationCreateArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST v1

		#region POST v1/ChatToExhibitor
		[HttpPost]
		[Route("v1/ChatToExhibitor")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> PostChatToExhibitor(
			MobileServiceNotificationCreateChatToExhibitorArguments arguments,
			[Injection] IServiceBaseHandler<MobileServiceNotificationCreateChatToExhibitorArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST v1/ChatToExhibitor

		#region POST v1/ChatToVisitor
		[HttpPost]
		[Route("v1/ChatToVisitor")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> PostChatToVisitor(
			MobileServiceNotificationCreateChatToVisitorArguments arguments,
			[Injection] IServiceBaseHandler<MobileServiceNotificationCreateChatToVisitorArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST v1/ChatToVisitor
	}
}
