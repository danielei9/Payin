using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/AccountLine")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.CommercePayment + "," + AccountRoles.Superadministrator + "," + AccountRoles.PaymentWorker
	)]
	public class ApiAccountLineController : ApiController
    {
        #region GET /Liquidation
        [HttpGet]
        [Route("Liquidation/{id}")]
        public async Task<ResultBase<AccountLineGetByLiquidationResult>> GetByLiquidation(
            int id,
            [FromUri] AccountLineGetByLiquidationArguments arguments,
            [Injection] IQueryBaseHandler<AccountLineGetByLiquidationArguments, AccountLineGetByLiquidationResult> handler
        )
        {
            arguments.LiquidationId = id;

            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /Liquidation

        #region GET /LogBook
        [HttpGet]
		[Route("LogBook")]
		public async Task<ResultBase<AccountLineGetByLogBookResult>> GetByLogBook(
			[FromUri] AccountLineGetByLogBookArguments arguments,
			[Injection] IQueryBaseHandler<AccountLineGetByLogBookArguments, AccountLineGetByLogBookResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /LogBook

        #region GET /Accounts
        [HttpGet]
        [Route("Accounts")]
        public async Task<ResultBase<AccountLineGetByAccountsResult>> GetByAccounts(
            [FromUri] AccountLineGetByAccountsArguments arguments,
            [Injection] IQueryBaseHandler<AccountLineGetByAccountsArguments, AccountLineGetByAccountsResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /Accounts

        #region GET /Cash
        [HttpGet]
        [Route("Cash")]
        public async Task<ResultBase<AccountLineGetByCashResult>> GetByCash(
            [FromUri] AccountLineGetByCashArguments arguments,
            [Injection] IQueryBaseHandler<AccountLineGetByCashArguments, AccountLineGetByCashResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /Cash

        #region GET /ServiceCards
        [HttpGet]
        [Route("ServiceCards")]
        public async Task<ResultBase<AccountLineGetByServiceCardsResult>> GetByServiceCards(
            [FromUri] AccountLineGetByServiceCardsArguments arguments,
            [Injection] IQueryBaseHandler<AccountLineGetByServiceCardsArguments, AccountLineGetByServiceCardsResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /ServiceCards

        #region GET /CreditCards
        [HttpGet]
        [Route("CreditCards")]
        public async Task<ResultBase<AccountLineGetByCreditCardsResult>> GetByCreditCards(
            [FromUri] AccountLineGetByCreditCardsArguments arguments,
            [Injection] IQueryBaseHandler<AccountLineGetByCreditCardsArguments, AccountLineGetByCreditCardsResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /CreditCards

        #region GET /Products
        [HttpGet]
        [Route("Products")]
        public async Task<ResultBase<AccountLineGetByProductsResult>> GetByProducts(
            [FromUri] AccountLineGetByProductsArguments arguments,
            [Injection] IQueryBaseHandler<AccountLineGetByProductsArguments, AccountLineGetByProductsResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /Products

        #region GET /EntranceTypes
        [HttpGet]
        [Route("EntranceTypes")]
        public async Task<ResultBase<AccountLineGetByEntranceTypesResult>> GetByEntranceTypes(
            [FromUri] AccountLineGetByEntranceTypesArguments arguments,
            [Injection] IQueryBaseHandler<AccountLineGetByEntranceTypesArguments, AccountLineGetByEntranceTypesResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /EntranceTypes

        #region GET /Others
        [HttpGet]
        [Route("Others")]
        public async Task<ResultBase<AccountLineGetByOthersResult>> GetByOthers(
            [FromUri] AccountLineGetByOthersArguments arguments,
            [Injection] IQueryBaseHandler<AccountLineGetByOthersArguments, AccountLineGetByOthersResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /Others
    }
}
