using PayIn.Application.Dto.Arguments.Device;
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
	[RoutePrefix("Mobile/Device")]
	[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative + "," + AccountClientId.AndroidFallesNative +"," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User
	)]
	public class MobileDeviceController : ApiController
	{
		#region POST /v1/
		[HttpPost]
		[Route("v1/")]
		public async Task<dynamic> Post(
			DeviceMobileCreateArguments arguments,
			[Injection] IServiceBaseHandler<DeviceMobileCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /v1/

		#region DELETE /v1/
		[HttpDelete]
		[Route("v1/")]
		public async Task<dynamic> Delete(
			[FromUri] DeviceMobileDeleteArguments arguments,
			[Injection] IServiceBaseHandler<DeviceMobileDeleteArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { };
		}
		#endregion DELETE /v1/
	}
}
