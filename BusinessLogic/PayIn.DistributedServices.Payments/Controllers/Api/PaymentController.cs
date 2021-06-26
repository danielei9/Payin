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
	[RoutePrefix("Api/Payment")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.Superadministrator + "," + AccountRoles.PaymentWorker
	)]
	public class PaymentController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<PaymentsGetAllResult>> GetAll(
			[FromUri] PaymentGetAllArguments arguments,
			[Injection] IQueryBaseHandler<PaymentGetAllArguments, PaymentsGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /Charges
		[HttpGet]
		[Route("Charges")]
		public async Task<ResultBase<PaymentGetChargesResult>> GetCharges(
			[FromUri] PaymentGetChargesArguments arguments,
			[Injection] IQueryBaseHandler<PaymentGetChargesArguments, PaymentGetChargesResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Charges

		#region GET /Unliquidated
		[HttpGet]
		[Route("Unliquidated")]
		public async Task<ResultBase<AccountLineGetAllUnliquidatedResult>> Unliquidated(
			[FromUri] AccountLineGetAllUnliquidatedArguments arguments,
			[Injection] IQueryBaseHandler<AccountLineGetAllUnliquidatedArguments, AccountLineGetAllUnliquidatedResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Unliquidated

		#region GET /Graph
		[HttpGet]
		[Route("Graph/")]
		public async Task<ResultBase<PaymentsGetGraphResult>> Graph(
			[FromUri] PaymentGetGraphArguments arguments,
			[Injection] IQueryBaseHandler<PaymentGetGraphArguments, PaymentsGetGraphResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Graph

		#region GET /LiquidationPayments
		[HttpGet]
		[Route("LiquidationPayments")]
		public async Task<ResultBase<PaymentsGetAllByLiquidationResult>> LiquidationPayments(
			[FromUri] PaymentGetAllByLiquidationArguments arguments,
			[Injection] IQueryBaseHandler<PaymentGetAllByLiquidationArguments, PaymentsGetAllByLiquidationResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /LiquidationPayments

		#region PUT /v1/Refund/{int:id}
		[HttpPut]
		[Route("v1/Refund/{id:int}")]
		public async Task<dynamic> Refund2(
			PaymentRefundArguments arguments,
			[Injection] IServiceBaseHandler<PaymentRefundArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /v1/Refund/{int:id}

		#region GET /CardPayments/{id:int}
		[HttpGet]
		[Route("CardPayments/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<PaymentGetAllByCardResult>> GetTickets(
			int id,
			[FromUri] PaymentGetAllByCardArguments argument,
			[Injection] IQueryBaseHandler<PaymentGetAllByCardArguments, PaymentGetAllByCardResult> handler)
		{
			argument.Id = id;
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /CardPayments/{id:int}

	}
}
