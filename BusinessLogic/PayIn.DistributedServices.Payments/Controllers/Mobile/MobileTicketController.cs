using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData.Query;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers
{
	[HideSwagger]
	[RoutePrefix("Mobile/Ticket")]
	public class MobileTicketController : ApiController
	{
		#region GET /v1/Dues
		[HttpGet]
		[Route("v1/Dues")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<ResultBase<TicketMobileGetDuesResult>> Get(
			[FromUri] TicketMobileGetDuesArguments arguments,
			ODataQueryOptions<TicketMobileGetDuesResult> options,
			[Injection] IQueryBaseHandler<TicketMobileGetDuesArguments, TicketMobileGetDuesResult> handler
		)
		{
			arguments.Skip = options.Skip == null ? 0 : options.Skip.Value;
			arguments.Top = Math.Min(options.Top == null ? 100 : options.Top.Value, 100);

			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/Dues

		#region GET /v1/{id:int}
		[HttpGet]
		[Route("v1/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<ResultBase<TicketMobileGetResult>> Get(
			int id,
			[FromUri] TicketMobileGetArguments arguments,
			[Injection] IQueryBaseHandler<TicketMobileGetArguments, TicketMobileGetResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/{id:int}

		#region GET /v1/Orders
		[HttpGet]
		[Route("v1/orders")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<ResultBase<MobileTicketGetOrdersResult>> GetOrders(
			[FromUri] MobileTicketGetOrdersArguments arguments,
			[Injection] IQueryBaseHandler<MobileTicketGetOrdersArguments, MobileTicketGetOrdersResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /v1/Orders

        #region GET /v1
        [HttpGet]
        [Route("v1")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<ResultBase<MobileTicketGetAllResult>> GetAll(
            [FromUri] MobileTicketGetAllArguments arguments,
            [Injection] IQueryBaseHandler<MobileTicketGetAllArguments, MobileTicketGetAllResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /v1

        #region GET /v1/OrdersByPaymentConcession/{paymentConcessionId}
        [HttpGet]
		[Route("v1/OrdersByPaymentConcession/{paymentConcessionId}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<ResultBase<MobileTicketGetOrdersResult>> GetOrderByPaymentConcessions(
			int paymentConcessionId,
			[FromUri] TicketMobileGetOrdersByPaymentConcessionArguments arguments,
			[Injection] IQueryBaseHandler<TicketMobileGetOrdersByPaymentConcessionArguments, MobileTicketGetOrdersResult> handler
		)
		{
			arguments.PaymentConcessionId = paymentConcessionId;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/OrdersByPaymentConcession/{paymentConcessionId}

		#region GET /v1/Pay/{id:int}
		[HttpGet]
		[Route("v1/Pay/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<ResultBase<TicketMobileGetPayResult>> GetPay(
			int id,
			[FromUri] TicketMobileGetPayArguments arguments,
			[Injection] IQueryBaseHandler<TicketMobileGetPayArguments, TicketMobileGetPayResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/Pay/{id:int}

		#region POST /v1
		[HttpPost]
		[Route("v1")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<dynamic> Post(
			TicketMobileCreateArguments arguments,
			[Injection] IServiceBaseHandler<TicketMobileCreateArguments> handler
		)
		{
#if PRODUCTION
			//throw new ApplicationException("En estos momentos estamos mejorando el sistema de pagos para incorporar el transporte público.\nEn unos días volverá a estar activo.\nDisculpen las molestias.");
#endif // PRODUCTION

			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /v1

		#region POST /v2
		[HttpPost]
		[Route("v2")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<dynamic> Postv2(
			MobileTicketCreateAndGetArguments arguments,
			[Injection] IServiceBaseHandler<MobileTicketCreateAndGetArguments> handler
		)
		{			
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v2

		#region POST /List
		[HttpPost]
        [Route("v1/List")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<dynamic> PostList(
            [FromBody]  MobileTicketCreateListArguments arguments,
            [Injection] IServiceBaseHandler<MobileTicketCreateListArguments> handler
        )
        {
            var item = await handler.ExecuteAsync(arguments);
            return item;
        }
        #endregion POST /List

        #region POST /v1/IFrame
        [HttpPost]
        [Route("v1/IFrame")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<dynamic> PostGetIFrame(
            TicketCreateAndGetIFrameArguments arguments,
            [Injection] IServiceBaseHandler<TicketCreateAndGetIFrameArguments> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion POST /v1/IFrame

        #region POST /v1/Pay
        [HttpPost]
		[Route("v1/Pay")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<dynamic> Pay(
			TicketMobilePayArguments arguments,
			[Injection] IServiceBaseHandler<TicketMobilePayArguments> handler
		)
		{
            var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
#endregion POST /v1/Pay

        #region POST /v2/Pay
		[HttpPost]
		[Route("v2/Pay")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<dynamic> PayV2(
			TicketMobilePayV2Arguments arguments,
			[Injection] IServiceBaseHandler<TicketMobilePayV2Arguments> handler
		)
		{
            var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /v1/Pay

		#region POST /v3/Pay
		[HttpPost]
		[Route("v3/Pay")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<dynamic> PayV3(
			MobileTicketPayV3Arguments arguments,
			[Injection] IServiceBaseHandler<MobileTicketPayV3Arguments> handler
		)
		{
            var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
        #endregion POST /v3/Pay

        #region POST /v1/AnswerForm/{id:int}
        [HttpPost]
        [Route("v1/AnswerForm/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<dynamic> AnswerForm(
            TicketAnswerFormsArguments arguments,
            [Injection] IServiceBaseHandler<TicketAnswerFormsArguments> handler,
            int id
        )
        {
            arguments.Id = id;
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion POST /v1/AnswerForm/{id:int}

        #region PUT /v1/UpdateStateAndGetOrders/{id:int}
        [HttpPut]
		[Route("v1/UpdateStateAndGetOrders/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<dynamic> GetUpdateStateAndGetOrders(
			int id,
			TicketMobileUpdateStateAndGetOrdersArguments arguments,
			[Injection] IServiceBaseHandler<TicketMobileUpdateStateAndGetOrdersArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /v1/UpdateStateAndGetOrders/{id:int}

		#region DELETE /v1
		[HttpDelete]
		[Route("v1")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp,
			Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<dynamic> Delete(
		    int id,
			[FromUri] TicketMobileDeleteArguments command,
			[Injection] IServiceBaseHandler<TicketMobileDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion DELETE /v1
	}
}
