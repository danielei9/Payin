using PayIn.Application.Dto.Payments.Arguments.PaymentUser;
using PayIn.Application.Dto.Payments.Results.PaymentUser;
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
	[RoutePrefix("Api/PaymentUser")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
	)]
	public class PaymentUserController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<PaymentUserGetAllResult>> GetAll(
			[FromUri] PaymentUserGetAllArguments arguments,
			[Injection] IQueryBaseHandler<PaymentUserGetAllArguments, PaymentUserGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<PaymentUserGetResult>> Get(
			[FromUri] PaymentUserGetArguments query,
			[Injection] IQueryBaseHandler<PaymentUserGetArguments, PaymentUserGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(query);
			return result;
		}
		#endregion  GET /{id:int}

		#region GET /Selector/{filter?}
		[HttpGet]

		[Route("Selector/{filter?}")]
		public async Task<ResultBase<PaymentUserGetSelectorResult>> Selector(
			string filter,
			[FromUri] PaymentUserGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<PaymentUserGetSelectorArguments, PaymentUserGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Selector/{filter?}

		#region POST /
		[HttpPost]
		[Route("")]		
		public async Task<dynamic> Post(
			PaymentUserCreateArguments arguments,
			[Injection] IServiceBaseHandler<PaymentUserCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]		
		public async Task<dynamic> Put(
			PaymentUserUpdateArguments command,
			[Injection] IServiceBaseHandler<PaymentUserUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region PUT /
		[HttpPut]
		[Route("Resend/{id:int}")]
		public async Task<dynamic> PUT(
			int id,
			PaymentUserResendNotificationArguments command,
			[Injection] IServiceBaseHandler<PaymentUserResendNotificationArguments> handler
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
			[FromUri] PaymentUserDeleteArguments arguments,
			[Injection] IServiceBaseHandler<PaymentUserDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}

	}	
}
