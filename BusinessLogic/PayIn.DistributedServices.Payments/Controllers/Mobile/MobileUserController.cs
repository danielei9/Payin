using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers
{
	[HideSwagger]
	[RoutePrefix("Mobile/User")]
	public class MobileUserController : ApiController
	{
		#region POST /v1
		[HttpPost]
		[Route("v1")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> Create(
			UserMobileCreateArguments command,
			[Injection] IServiceBaseHandler<UserMobileCreateArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return new { Id = result };
		}
		#endregion POST /v1

		#region POST /v1/Pin
		[HttpPost]
		[Route("v1/Pin")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.FallasProApp,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> UpdatePin(
			UserMobileUpdatePinArguments command,
			[Injection] IServiceBaseHandler<UserMobileUpdatePinArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return new { Id = result };
		}
        #endregion POST /v1/Pin

        #region POST /v1/ForgotPin
        [HttpPost]
        [Route("v1/ForgotPin")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> ForgotPin(
            UserMobileForgotPinArguments command,
            [Injection] IServiceBaseHandler<UserMobileForgotPinArguments> handler
        )
        {
            var result = await handler.ExecuteAsync(command);
            return new { Id = result };
        }
        #endregion POST /v1/Pin

        #region GET /v1/HasPayment
        [HttpGet]
		[Route("v1/HasPayment")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> HasPayment(
			[FromUri] UserMobileHasPaymentArguments command,
			[Injection] IServiceBaseHandler<UserMobileHasPaymentArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return new { HasPayment = result };
		}
		#endregion GET /v1/HasPayment
	}
}
