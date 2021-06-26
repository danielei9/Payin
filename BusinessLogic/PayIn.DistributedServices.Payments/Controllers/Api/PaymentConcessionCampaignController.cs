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
	[RoutePrefix("Api/PaymentConcessionCampaign")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
	)]
	public class PaymentConcessionCampaignController : ApiController
	{
		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<PaymentConcessionCampaignGetAllResult>> GetAll(
			[FromUri] PaymentConcessionCampaignGetAllArguments arguments,
			[Injection] IQueryBaseHandler<PaymentConcessionCampaignGetAllArguments, PaymentConcessionCampaignGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /	
		#region GET
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<PaymentConcessionCampaignGetResult>> Get(
			[FromUri] PaymentConcessionCampaignGetArguments arguments,
			[Injection] IQueryBaseHandler<PaymentConcessionCampaignGetArguments, PaymentConcessionCampaignGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /	
		
		#region POST /
		[HttpPost]
		[Route("Create/{id:int}")]
		public async Task<dynamic> Post(
			PaymentConcessionCampaignCreateArguments arguments,
			[Injection] IServiceBaseHandler<PaymentConcessionCampaignCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /

		#region PUT /
		[HttpPut]
		[Route("Resend/{id:int}")]
		public async Task<dynamic> PUT(
			int id,
			PaymentConcessionCampaignResendNotificationArguments command,
			[Injection] IServiceBaseHandler<PaymentConcessionCampaignResendNotificationArguments> handler
		)
		{
			command.Id = id;
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion PUT /

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] PaymentConcessionCampaignDeleteArguments arguments,
			[Injection] IServiceBaseHandler<PaymentConcessionCampaignDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}
		
	}
}
