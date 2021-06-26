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
	[RoutePrefix("Api/Liquidation")]
	[XpAuthorize(
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.CommercePayment + "," + AccountRoles.Transport
	)]
	public class LiquidationController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<LiquidationGetAllResult>> GetAll(
			[FromUri] LiquidationGetAllArguments arguments,
			[Injection] IQueryBaseHandler<LiquidationGetAllArguments, LiquidationGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region POST /Create
		[HttpPost]
		[Route("Create/")]
		public async Task<dynamic> Create(
			LiquidationCreateArguments arguments,
			[Injection] IServiceBaseHandler<LiquidationCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion POST /Create

		#region GET /Liquidations
		[HttpGet]
		[Route("Liquidations/")]
		public async Task<ResultBase<LiquidationPayResult>> LiquidationPay(
			[FromUri] LiquidationPayArguments arguments,
			[Injection] IQueryBaseHandler<LiquidationPayArguments, LiquidationPayResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Liquidations

		#region PUT /Change
		[HttpPut]
		[Route("Change/")]
		public async Task<dynamic> Put(
			[FromUri] LiquidationChangeArguments arguments,
			[Injection] IServiceBaseHandler<LiquidationChangeArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
        #endregion PUT /Change

        #region PUT /Open/{id}
        [HttpPut]
		[Route("Open/{id}")]
		public async Task<dynamic> Open(
            int id,
            [FromUri] LiquidationOpenArguments arguments,
			[Injection] IServiceBaseHandler<LiquidationOpenArguments> handler
		)
        {
            arguments.Id = id;

            var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
        #endregion PUT /Open/{id}

        #region PUT /Close/{id}
        [HttpPut]
        [Route("Close/{id}")]
        public async Task<dynamic> Close(
            int id,
            [FromUri] LiquidationCloseArguments arguments,
            [Injection] IServiceBaseHandler<LiquidationCloseArguments> handler
        )
        {
            arguments.Id = id;

            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion PUT /Close/{id}

        #region POST /CreateAndPay
        [HttpPost]
        [Route("CreateAndPay")]
        public async Task<dynamic> CreateAndPay(
            [FromBody] LiquidationCreateAndPayArguments arguments,
            [Injection] IServiceBaseHandler<LiquidationCreateAndPayArguments> handler
        )
        {
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion POST /CreateAndPay

        #region PUT /Pay
        [HttpPut]
		[Route("Pay/{id:int}")]
		public async Task<dynamic> Pay(
            int id,
			[FromUri] LiquidationPayArguments arguments,
			[Injection] IServiceBaseHandler<LiquidationPayArguments> handler
		)
		{
            arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /Pay

		#region GET /SelectorConcession/{filter?}
		[HttpGet]
		[Route("SelectorConcession/{filter?}")]
		public async Task<ResultBase<LiquidationGetPaymentConcessionSelectorResult>> RetrieveSelector(
			string filter,
			[FromUri]  LiquidationGetPaymentConcessionSelectorArguments command,
			[Injection] IQueryBaseHandler<LiquidationGetPaymentConcessionSelectorArguments, LiquidationGetPaymentConcessionSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
        #endregion GET /SelectorConcession/{filter?}

        #region GET /SelectorOpened/{filter?}
        [HttpGet]
        [Route("SelectorOpened/{filter?}")]
        public async Task<ResultBase<SelectorResult>> GetSelectorOpened(
            string filter,
            [FromUri]  LiquidationGetSelectorOpenedArguments command,
            [Injection] IQueryBaseHandler<LiquidationGetSelectorOpenedArguments, SelectorResult> handler
        )
        {
            var result = await handler.ExecuteAsync(command);
            return result;
        }
        #endregion GET /SelectorOpened/{filter?}
        
        #region POST /AccountLines
        [HttpPost]
        [Route("AccountLines")]
        public async Task<dynamic> CreateAccountLines(
            [FromBody] LiquidationCreateAccountLinesArguments arguments,
            [Injection] IServiceBaseHandler<LiquidationCreateAccountLinesArguments> handler
        )
        {
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion POST /AccountLines

        #region PUT /AddAccountLines/{id}
        [HttpPut]
        [Route("AddAccountLines/{id}")]
        public async Task<dynamic> AddAccountLines(
            int id,
            [FromBody] LiquidationAddAccountLinesArguments arguments,
            [Injection] IServiceBaseHandler<LiquidationAddAccountLinesArguments> handler
        )
        {
            arguments.Id = id;
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion PUT /AddAccountLines/{id}

        #region PUT /RemoveAccountLines/{id}
        [HttpPut]
        [Route("RemoveAccountLines/{id}")]
        public async Task<dynamic> DeleteAccountLines(
            int id,
            [FromBody] LiquidationRemoveAccountLinesArguments arguments,
            [Injection] IServiceBaseHandler<LiquidationRemoveAccountLinesArguments> handler
        )
        {
            arguments.Id = id;
            await handler.ExecuteAsync(arguments);
            return new { id };
        }
        #endregion PUT /RemoveAccountLines/{id}
    }
}
