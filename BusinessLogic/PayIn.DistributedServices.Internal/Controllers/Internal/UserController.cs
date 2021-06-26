using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Internal.Controllers.Internal
{
	[HideSwagger]
	[RoutePrefix("Internal/User")]
	public class UserController : ApiController
	{
		#region POST /
		[HttpPost]
		[Route("")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.Web + "," + AccountClientId.PaymentApi + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> Create(
			UserCreateArguments arguments,
			[Injection] IServiceBaseHandler<UserCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return null;
		}
		#endregion POST /

		#region POST /Pin
		[HttpPost]
		[Route("Pin")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> UpdatePin(
			UserUpdatePinArguments arguments,
			[Injection] IServiceBaseHandler<UserUpdatePinArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return null;
		}
        #endregion POST /Pin

        #region POST /ForgotPin
        [HttpPost]
        [Route("ForgotPin")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative,
            Roles = AccountRoles.User
        )]
        public async Task<dynamic> ForgotPin(
            UserForgotPinArguments arguments,
            [Injection] IServiceBaseHandler<UserForgotPinArguments> handler
        )
        {
            var item = await handler.ExecuteAsync(arguments);
            return item;
        }
        #endregion POST /Pin

        #region GET /CheckPin
        [HttpGet]
		[Route("CheckPin")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative + "," + AccountClientId.Web + "," + AccountClientId.PaymentApi + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> CheckPin(
			[FromUri] UserCheckPinArguments arguments,
			[Injection] IServiceBaseHandler<UserCheckPinArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { CheckPin = item };
		}
		#endregion GET /CheckPin

		#region GET /HasPayment
		[HttpGet]
		[Route("HasPayment")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative + "," + AccountClientId.Web + "," + AccountClientId.PaymentApi + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> HasPayment(
			[FromUri]   UserHasPaymentArguments arguments,
			[Injection] IServiceBaseHandler<UserHasPaymentArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { HasPayment = item };
		}
		#endregion GET /HasPayment
	}
}
