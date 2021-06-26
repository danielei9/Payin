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
	[RoutePrefix("Internal/Ticket")]
	public class TicketController : ApiController
	{
        #region POST /PayWeb
        [HttpPost]
		[Route("PayWeb")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative + "," + AccountClientId.Web + "," + AccountClientId.PaymentApi,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> PayWeb(
			TicketPayWebArguments arguments,
			[Injection] IServiceBaseHandler<TicketPayWebArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
        #endregion POST /PayWeb
    }
}
