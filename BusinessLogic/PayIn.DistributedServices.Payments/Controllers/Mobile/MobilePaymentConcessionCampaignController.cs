using PayIn.Application.Dto.Payments.Arguments;
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
	[RoutePrefix("Mobile/PaymentConcessionCampaign")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class MobilePaymentConcessionCampaignController : ApiController
	{
		#region PUT /v1/AcceptAssignment/{id:int}
		[HttpPut]
		[Route("v1/AcceptAssignment/{id:int}")]
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
	}
}