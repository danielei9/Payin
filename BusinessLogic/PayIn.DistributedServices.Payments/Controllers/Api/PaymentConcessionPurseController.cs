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
	[RoutePrefix("Api/PaymentConcessionPurse")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
	)]
	public class PaymentConcessionPurseController : ApiController
	{
		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<PaymentConcessionPurseGetAllResult>> GetAll(
			[FromUri] PaymentConcessionPurseGetAllArguments arguments,
			[Injection] IQueryBaseHandler<PaymentConcessionPurseGetAllArguments, PaymentConcessionPurseGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region PUT /
		[HttpPut]
		[Route("Resend/{id:int}")]
		public async Task<dynamic> PUT(
			int id,
			PaymentConcessionPurseResendNotificationArguments command,
			[Injection] IServiceBaseHandler<PaymentConcessionPurseResendNotificationArguments> handler
		)
		{
			command.Id = id;
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion PUT /
		
		#region Delete /{id:int}
		[HttpDelete]
		[Route("Delete/{id:int}")]
		public async Task<dynamic> Post(
			[FromUri] PaymentConcessionPurseDeleteArguments arguments,
			[Injection] IServiceBaseHandler<PaymentConcessionPurseDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion Delete /{id:int}

		#region POST /
		[HttpPost]
		[Route("Create/{id:int}")]
		public async Task<dynamic> Post(
			PaymentConcessionPurseCreateArguments arguments,
			[Injection] IServiceBaseHandler<PaymentConcessionPurseCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /

	}

}
