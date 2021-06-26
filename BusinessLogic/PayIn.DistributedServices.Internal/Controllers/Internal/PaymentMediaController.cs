using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Application.Dto.Internal.Results;
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
	[RoutePrefix("Internal/PaymentMedia")]
	public class PaymentMediaController : ApiController
	{
		#region POST /WebCard
		[HttpPost]
		[Route("WebCard")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative + "," + AccountClientId.Web + "," + AccountClientId.PaymentApi,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> CreateWebCard(
			PaymentMediaCreateWebCardArguments arguments,
			[Injection] IServiceBaseHandler<PaymentMediaCreateWebCardArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion POST /WebCard

		#region POST /WebCardRefundSabadell
		[HttpPost]
		[Route("WebCardRefundSabadell")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.Web + "," + AccountClientId.PaymentApi,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> WebCardRefundSabadell(
			PaymentMediaWebCardRefundSabadellArguments arguments,
			[Injection] IServiceBaseHandler<PaymentMediaWebCardRefundSabadellArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion POST /WebCardRefundSabadell

		#region POST /Pay
		[HttpPost]
		[Route("Pay")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative + "," + AccountClientId.Web + "," + AccountClientId.PaymentApi,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> Pay(
			PaymentMediaPayArguments arguments,
			[Injection] IServiceBaseHandler<PaymentMediaPayArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
        #endregion POST /Pay

        #region POST /Recharge
        [HttpPost]
        [Route("Recharge")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.Web + "," + AccountClientId.PaymentApi,
			Roles = AccountRoles.User
		)]
        public async Task<dynamic> Recharge(
            PaymentMediaRechargeArguments arguments,
            [Injection] IServiceBaseHandler<PaymentMediaRechargeArguments> handler
        )
        {
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
		#endregion POST /Recharge

		#region GET /GetBalance
		[HttpGet]
        [Route("GetBalance/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative + "," + AccountClientId.Web + "," + AccountClientId.PaymentApi,
			Roles = AccountRoles.User
		)]
        public async Task<ResultBase<PaymentMediaGetBalanceResult>> GetBalance(
            int id,
            [FromUri] PaymentMediaGetBalanceArguments arguments,
            [Injection] IQueryBaseHandler<PaymentMediaGetBalanceArguments, PaymentMediaGetBalanceResult> handler
        )
        {
            arguments.PublicId = id;
            var item = await handler.ExecuteAsync(arguments);
            return new ResultBase<PaymentMediaGetBalanceResult>() { Data = item.Data };
        }
		#endregion GET /GetBalance

		#region GET /GetBalanceToRefund
		[HttpGet]
		[Route("GetBalanceToRefund/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.Web + "," + AccountClientId.PaymentApi,
			Roles = AccountRoles.User
		)]
		public async Task<ResultBase<PaymentMediaGetBalanceToRefundResult>> GetBalance2(
			int id,
			[FromUri] PaymentMediaGetBalanceToRefundArguments arguments,
			[Injection] IQueryBaseHandler<PaymentMediaGetBalanceToRefundArguments, PaymentMediaGetBalanceToRefundResult> handler
		)
		{
			arguments.PublicId = id;
			var item = await handler.ExecuteAsync(arguments);
			return new ResultBase<PaymentMediaGetBalanceToRefundResult>() { Data = item.Data };
		}
		#endregion GET /GetBalance

		#region POST /PaySabadell
		[HttpPost]
		[Route("PaySabadell")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.Web + "," + AccountClientId.PaymentApi,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> PaySabadell(
			PaymentMediaPaySabadellArguments arguments,
			[Injection] IServiceBaseHandler<PaymentMediaPaySabadellArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion POST /PaySabadell

		#region POST /Refund
		[HttpPost]
		[Route("Refund")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.Web + "," + AccountClientId.PaymentApi,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> Refund(
			PaymentMediaRefundArguments arguments,
			[Injection] IServiceBaseHandler<PaymentMediaRefundArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
        #endregion POST /Refund

        #region DELETE /{id:int}
        [HttpDelete]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative + "," + AccountClientId.Web + "," + AccountClientId.PaymentApi,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> Delete(
			int id,
			[FromUri] PaymentMediaDeleteArguments command,
			[Injection] IServiceBaseHandler<PaymentMediaDeleteArguments> handler
		)
		{
			command.PublicId = id;

			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion DELETE /{id:int}
	}
}
