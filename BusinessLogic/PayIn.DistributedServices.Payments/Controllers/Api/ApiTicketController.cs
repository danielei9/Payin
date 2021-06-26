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
	[RoutePrefix("Api/Ticket")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.Superadministrator + "," + AccountRoles.PaymentWorker
	)]
	public class ApiTicketController : ApiController
	{
		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<TicketGetResult>> Get(
			int id,
			[FromUri] TicketGetArguments arguments,
			[Injection] IQueryBaseHandler<TicketGetArguments, TicketGetResult> handler
		)
		{
			arguments.Id = id;

			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<TicketGetAllResult>> GetAll(
			[FromUri] TicketGetAllArguments arguments,
			[Injection] IQueryBaseHandler<TicketGetAllArguments, TicketGetAllResult> handler
		)
		{		
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /

        #region GET /SystemCard
        [HttpGet]
        [Route("SystemCard")]
        public async Task<ResultBase<TicketGetAllResult>> GetSystemCard(
            [FromUri] TicketGetSystemCardArguments arguments,
            [Injection] IQueryBaseHandler<TicketGetSystemCardArguments, TicketGetAllResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /SystemCard

        #region GET /Details
        [HttpGet]
		[Route("Details/{id:int}")]
		public async Task<ResultBase<TicketGetDetailsResult>> GetDetails(
			int id,
			[FromUri] TicketGetDetailsArguments arguments,
			[Injection] IQueryBaseHandler<TicketGetDetailsArguments, TicketGetDetailsResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Details

		#region POST /
		[HttpPost]
		[Route("")]
		public async Task<dynamic> Post(
			TicketCreateArguments command,
			[Injection] IServiceBaseHandler<TicketCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion POST /

		#region DELETE /
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			int id,
			[FromUri] TicketDeleteArguments command,
			[Injection] IServiceBaseHandler<TicketDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion DELETE /

		#region GET /Graph
		[HttpGet]
		[Route("Graph/")]
		public async Task<ResultBase<TicketGraphResult>> Graph(
			[FromUri] TicketGraphArguments arguments,
			[Injection] IQueryBaseHandler<TicketGraphArguments, TicketGraphResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Graph

		#region PUT /Activate
		[HttpPut]
		[Route("Activate")]
		public async Task<dynamic> Activate(
			TicketActivateArguments command,
			[Injection] IServiceBaseHandler<TicketActivateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return item;
		}
		#endregion PUT /Activate
		
		#region POST /SellCreateAndGet
		[HttpPost]
		[Route("SellCreateAndGet")]
		public async Task<dynamic> CreateAndGet(
			MobileTicketCreateAndGetArguments arguments,
			[Injection] IServiceBaseHandler<MobileTicketCreateAndGetArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /SellCreateAndGet

		#region POST /SellEntrances
		[HttpPost]
		[Route("SellEntrances")]
		public async Task<dynamic> SellEntrances(
			[FromBody] TicketSellArguments arguments,
			[Injection] IServiceBaseHandler<TicketSellArguments> handler)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
        #endregion POST /SellEntrances

        #region POST /GiveEntrances
        [HttpPost]
		[Route("GiveEntrances")]
		public async Task<dynamic> GiveEntrances(
			[FromBody] TicketGiveEntrancesArguments arguments,
			[Injection] IServiceBaseHandler<TicketGiveEntrancesArguments> handler)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
			
		}
		#endregion POST /GiveEntrances

		#region POST /RechargeBalance
		[HttpPost]
		[Route("RechargeBalance")]
		public async Task<dynamic> RechargeBalance(
			//int id,
			[FromBody] TicketRechargeBalanceArguments arguments,
			[Injection] IServiceBaseHandler<TicketRechargeBalanceArguments> handler)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /RechargeBalance

		#region POST /DonateBalance
		[HttpPost]
		[Route("DonateBalance")]
		public async Task<dynamic> DonateBalance(
			[FromBody] TicketDonateBalanceArguments arguments,
			[Injection] IServiceBaseHandler<TicketDonateBalanceArguments> handler)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
        #endregion POST /DonateBalance/{id:int}

        #region POST /BuyEntrances
        [HttpPost]
        [Route("BuyEntrances")]
        public async Task<dynamic> BuyEntrances(
            [FromBody] TicketBuyEntrancesArguments arguments,
            [Injection] IServiceBaseHandler<TicketBuyEntrancesArguments> handler)
        {
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
		#endregion POST /BuyEntrances

		#region POST /BuyManyEntrances/{id}
		[HttpPost]
		[Route("BuyManyEntrances/{id}")]
		public async Task<dynamic> BuyManyEntrances(
			int id,
			[FromBody] TicketBuyManyEntrancesArguments arguments,
			[Injection] IServiceBaseHandler<TicketBuyManyEntrancesArguments> handler)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /BuyManyEntrances/{id}

		#region POST /BuyManyProducts/{id}
		[HttpPost]
		[Route("BuyManyProducts/{id}")]
		public async Task<dynamic> BuyManyProducts(
			int id,
			[FromBody] TicketBuyManyProductsArguments arguments,
			[Injection] IServiceBaseHandler<TicketBuyManyProductsArguments> handler)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /BuyManyProducts/{id}

		#region POST /BuyRechargeBalance
		[HttpPost]
		[Route("BuyRechargeBalance")]
		public async Task<dynamic> BuyRechargeBalance(
			[FromBody] TicketBuyRechargeBalanceArguments arguments,
			[Injection] IServiceBaseHandler<TicketBuyRechargeBalanceArguments> handler)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /BuyRechargeBalance

        #region POST /Pay/{id}
        [HttpPost]
        [Route("Pay/{id}")]
        public async Task<dynamic> Pay(
            int id,
            ApiTicketPayArguments arguments,
            [Injection] IServiceBaseHandler<ApiTicketPayArguments> handler
        )
        {
            arguments.Id = id;

            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion POST /Pay/{id}
    }
}
