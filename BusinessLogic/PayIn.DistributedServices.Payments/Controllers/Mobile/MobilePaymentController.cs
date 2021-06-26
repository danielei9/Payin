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
	[RoutePrefix("Mobile/Payment")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
	)]
	public class MobilePaymentController : ApiController
	{
		#region POST /v1/Refund
		[HttpPost]
		[Route("v1/Refund")]
		public async Task<dynamic> Refund(
			PaymentRefundArguments arguments,
			[Injection] IServiceBaseHandler<PaymentRefundArguments> handler
		)
        {
            var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /v1/Refund

		#region POST /v1/Refund/{int:id}
		[HttpPost]
		[Route("v1/Refund/{id:int}")]
		public async Task<dynamic> Refund2(
			PaymentRefundArguments arguments,
			[Injection] IServiceBaseHandler<PaymentRefundArguments> handler
		)
        {
            var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /v1/Refund/{int:id}
	}
}
