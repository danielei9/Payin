using PayIn.Application.Dto.Payments.Arguments.Payment;
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
	[RoutePrefix("Tpv/Payment")]
	[XpAuthorize(
		ClientIds = AccountClientId.Tpv,
		Roles = AccountRoles.PaymentWorker + "," + AccountRoles.CommercePayment
	)]
	public class TpvPaymentController : ApiController
	{
		#region POST /v1/Refund
		[HttpPost]
		[Route("v1/Refund")]
		public async Task<dynamic> Refund(
			PaymentTpvRefundArguments arguments,
			[Injection] IServiceBaseHandler<PaymentTpvRefundArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /v1/Refund
	}
}