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
	[RoutePrefix("Mobile/PaymentConcession")]
	// XpAuthorize en métodos
	public class MobilePaymentConcessionController : ApiController
	{
		#region GET /v1
		[HttpGet]
		[Route("v1")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative,
			Roles = AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<ResultBase<PaymentConcessionMobileGetAllResult>> Get(
			[FromUri] PaymentConcessionMobileGetAllArguments arguments,
			[Injection] IQueryBaseHandler<PaymentConcessionMobileGetAllArguments, PaymentConcessionMobileGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1

		#region PUT /v1/AcceptAssignment/{id:int}
		[HttpPut]
		[Route("v1/AcceptAssignment/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative,
			Roles = AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<dynamic> AcceptAssignment(
			int id,
			PaymentConcessionCampaignMobileAcceptAssignmentArguments command,
			[Injection] IServiceBaseHandler<PaymentConcessionCampaignMobileAcceptAssignmentArguments> handler
		)
		{
			command.Id = id;
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion PUT /v1/AcceptAssignment/{id:int}

		#region PUT /v1/RejectAssignment/{id:int}
		[HttpPut]
		[Route("v1/RejectAssignment/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative,
			Roles = AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
		)]
		public async Task<dynamic> RejectAssignment(
			int id,
			PaymentConcessionCampaignMobileRejectAssignmentArguments arguments,
			[Injection] IServiceBaseHandler<PaymentConcessionCampaignMobileRejectAssignmentArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return new { Id = result.Id };
		}
		#endregion PUT /v1/RejectAssignment/{id:int}

		#region GET /v1/WithPromotions
		[HttpGet]
		[Route("v1/WithPromotions")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative,
			Roles = AccountRoles.User
		)]
		public async Task<ResultBase<PaymentConcessionMobileGetAllWithPromotionsResult>> Get(
			[FromUri] PaymentConcessionMobileGetAllWithPromotionsArguments arguments,
			[Injection] IQueryBaseHandler<PaymentConcessionMobileGetAllWithPromotionsArguments, PaymentConcessionMobileGetAllWithPromotionsResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/WithPromotions

		#region GET /v1/{id:int}
		[HttpGet]
		[Route("v1/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative,
			Roles = AccountRoles.User
		)]
		public async Task<ResultBase<PaymentConcessionMobileGetResult>> Get(
			int id,
			[FromUri] PaymentConcessionMobileGetArguments arguments,
			[Injection] IQueryBaseHandler<PaymentConcessionMobileGetArguments, PaymentConcessionMobileGetResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/{id:int}

		#region GET /v1/ServiceCardsByUid
		[HttpGet]
		[Route("v1/ServiceCardsByUid")]
		public async Task<ResultBase<PaymentConcessionMobileGetServiceCardsByUidResult>> GetByUid(
			[FromUri] PaymentConcessionMobileGetServiceCardsByUidArguments arguments,
			[Injection] IQueryBaseHandler<PaymentConcessionMobileGetServiceCardsByUidArguments, PaymentConcessionMobileGetServiceCardsByUidResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/ServiceCardsByUid
	}
}
