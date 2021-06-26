using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers.Mobile
{
	[HideSwagger]
	[RoutePrefix("Mobile/Entrance")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative + "," + AccountClientId.AndroidFallesNative,
		Roles = AccountRoles.User
	)]
	public class MobileEntranceController : ApiController
	{
		#region GET /v1/{id:int}
		[HttpGet]
		[Route("v1/{id:int}")]
		public async Task<ResultBase<MobileEntranceGetResult>> Get(
			int id,
			[FromUri] MobileEntranceGetArguments arguments,
			[Injection] IQueryBaseHandler<MobileEntranceGetArguments, MobileEntranceGetResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /v1/{id:int}		

        #region GET /v1
        [HttpGet]
        [Route("v1")]
        public async Task<ResultBase<MobileEntranceGetAllResult>> GetAll(
            [FromUri] MobileEntranceGetAllArguments arguments,
            [Injection] IQueryBaseHandler<MobileEntranceGetAllArguments, MobileEntranceGetAllResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /v1	

        #region POST /v1/loadQr
        [HttpPost]
		[Route("v1/loadQr")]
		public async Task<dynamic> LoadQr(
			[FromBody] MobileEntranceLoadQrArguments arguments,
			[Injection] IServiceBaseHandler<MobileEntranceLoadQrArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /v1/loadQr	

		#region POST /v1/loadText
		[HttpPost]
		[Route("v1/loadText")]
		public async Task<dynamic> LoadText(
			[FromBody] MobileEntranceLoadTextArguments arguments,
			[Injection] IServiceBaseHandler<MobileEntranceLoadTextArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /v1/loadText

		#region GET /v1/ByPaymentConcession/{paymentConcessionId}
		[HttpGet]
		[Route("v1/ByPaymentConcession/{paymentConcessionId}")]
		public async Task<ResultBase<MobilePaymentMediaGetAllResult>> GetByPaymentConcession(
			int paymentConcessionId,
			[FromUri] MobileEntranceGetByPaymentConcessionArguments arguments,
			[Injection] IQueryBaseHandler<MobileEntranceGetByPaymentConcessionArguments, MobilePaymentMediaGetAllResult> handler
		)
		{
			arguments.PaymentConcessionId = paymentConcessionId;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/ByPaymentConcession/{paymentConcessionId}

		#region GET /v1/ByEvent/{eventId}
		[HttpGet]
		[Route("v1/ByEvent/{eventId}")]
		public async Task<ResultBase<MobilePaymentMediaGetAllResult>> GetByEvent(
			int eventId,
			[FromUri] MobileEntranceGetByEventArguments arguments,
			[Injection] IQueryBaseHandler<MobileEntranceGetByEventArguments, MobilePaymentMediaGetAllResult> handler
		)
		{
			arguments.EventId = eventId;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/ByEvent/{eventId}

		#region GET /v1/BySystemCard/{systemCardId}
		[HttpGet]
		[Route("v1/BySystemCard/{systemCardId}")]
		public async Task<ResultBase<MobilePaymentMediaGetAllResult>> GetBySystemCard(
			int systemCardId,
			[FromUri] MobileEntranceGetBySystemCardArguments arguments,
			[Injection] IQueryBaseHandler<MobileEntranceGetBySystemCardArguments, MobilePaymentMediaGetAllResult> handler
		)
		{
			arguments.SystemCardId = systemCardId;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/BySystemCard/{systemCardId}

		#region GET /v1/Active
		[HttpGet]
		[Route("v1/Active")]
		public async Task<ResultBase<MobileEntranceGetActiveResult>> GetAll(
			[FromUri] MobileEntranceGetActiveArguments arguments,
			[Injection] IQueryBaseHandler<MobileEntranceGetActiveArguments, MobileEntranceGetActiveResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/Active
	}
}