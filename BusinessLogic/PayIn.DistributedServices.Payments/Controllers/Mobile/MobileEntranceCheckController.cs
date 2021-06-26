using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.Application.Results;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers
{
	[HideSwagger]
	[RoutePrefix("mobile/entrancecheck")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
		Roles = AccountRoles.User
	)]
	public class MobileEntranceCheckController : ApiController
	{
		#region POST /v1/qr
		[HttpPost]
		[Route("v1/qr")]
		public async Task<dynamic> PostQr(
			[FromBody] MobileEntranceCheckCreateQrArguments arguments,
			[Injection] IServiceBaseHandler<MobileEntranceCheckCreateQrArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion POST /v1/qr

        #region POST /v1/text
        [HttpPost]
        [Route("v1/text")]
        public async Task<dynamic> PostText(
            [FromBody] MobileEntranceCheckCreateTextArguments arguments,
            [Injection] IServiceBaseHandler<MobileEntranceCheckCreateTextArguments> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion POST /v1/text
    }
}
