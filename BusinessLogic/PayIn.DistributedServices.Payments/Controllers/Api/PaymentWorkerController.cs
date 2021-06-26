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
	[RoutePrefix("Api/PaymentWorker")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
	)]
	public class PaymentWorkerController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<PaymentWorkerGetAllResult>> GetAll(
			[FromUri] PaymentWorkerGetAllArguments arguments,
			[Injection] IQueryBaseHandler<PaymentWorkerGetAllArguments, PaymentWorkerGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /Concession
		[HttpGet]
		[Route("Concession/")]
		public async Task<ResultBase<PaymentWorkerGetAllConcessionResult>> GetAll(
			[FromUri] PaymentWorkerGetAllConcessionArguments arguments,
			[Injection] IQueryBaseHandler<PaymentWorkerGetAllConcessionArguments, PaymentWorkerGetAllConcessionResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Concession

		#region POST /
		[HttpPost]
		[Route("")]
		public async Task<dynamic> Post(
			PaymentWorkerCreateArguments command,
			[Injection] IServiceBaseHandler<PaymentWorkerCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion POST /

		#region PUT /
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> PUT(
            int id,
			PaymentWorkerResendNotificationArguments command,
			[Injection] IServiceBaseHandler<PaymentWorkerResendNotificationArguments> handler
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
			[FromUri] PaymentWorkerDeleteArguments arguments,
			[Injection] IServiceBaseHandler<PaymentWorkerDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}

		#region DELETE DissociateConcession/{id:int}
		[HttpDelete]
		[Route("DissociateConcession/{id:int}")]
		public async Task<dynamic> DissociateConcession(
			[FromUri] PaymentWorkerDissociateConcessionArguments arguments,
			[Injection] IServiceBaseHandler<PaymentWorkerDissociateConcessionArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE DissociateConcession/{id:int}
	}
}
