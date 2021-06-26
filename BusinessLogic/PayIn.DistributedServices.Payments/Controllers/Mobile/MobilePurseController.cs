using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
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
    [RoutePrefix("Mobile/Purse")]
    public class MobilePurseController : ApiController
    {
        #region GET /v1/Buyable
        [HttpGet]
        [Route("v1/Buyable")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.User
		)]
		public async Task<ResultBase<MobilePurseGetBuyableResult>> GetBuyable(
            [FromUri] MobilePurseGetBuyableArguments arguments,
            [Injection] IQueryBaseHandler<MobilePurseGetBuyableArguments, MobilePurseGetBuyableResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion GET /v1/Buyable

		#region GET /v1/Sellable
		[HttpGet]
		[Route("v1/Sellable")]
		[XpAuthorize(
			ClientIds = AccountClientId.CashlessProApp,
			Roles = AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorkerCash
		)]
		public async Task<ResultBase<MobilePurseGetSellableResult>> GetSellable(
			[FromUri] MobilePurseGetSellableArguments arguments,
			[Injection] IQueryBaseHandler<MobilePurseGetSellableArguments, MobilePurseGetSellableResult> handler
		)
        {
            var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/Sellable
	}
}
