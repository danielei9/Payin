using PayIn.Application.Dto.Internal.Arguments;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Internal.Controllers.Sabadell
{
	[HideSwagger]
	[RoutePrefix("Sabadell")]
	public class SabadellController : ApiController
	{
		#region POST /WebCard
		[HttpPost]
		[Route("WebCard")]
		public async Task<dynamic> WebCard(
			PaymentMediaCreateWebCardSabadellArguments arguments,
			[Injection] IServiceBaseHandler<PaymentMediaCreateWebCardSabadellArguments> handler
		)
		{
			await handler.ExecuteAsync(arguments);
			return new { };
		}
		#endregion POST /WebCard

		#region POST /WebCardRefund
		[HttpPost]
		[Route("WebCardRefund")]
		public async Task<dynamic> WebCardRefund(
			PaymentMediaCreateWebCardRefundSabadellArguments arguments,
			[Injection] IServiceBaseHandler<PaymentMediaCreateWebCardRefundSabadellArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion POST /WebCardRefund

        #region POST /WebCardConfirm
        [HttpPost]
        [Route("WebCardConfirm")]
        public async Task<dynamic> WebCardConfirm(
            PaymentMediaCreateWebCardConfirmSabadellArguments arguments,
            [Injection] IServiceBaseHandler<PaymentMediaCreateWebCardConfirmSabadellArguments> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return new { Id = result.Id };
        }
        #endregion POST /WebCardRefund
    }
}
