using PayIn.Application.Dto.Arguments.ControlForm;
using PayIn.Application.Dto.Results.ControlForm;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/ControlForm")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.PaymentWorker + "," + AccountRoles.CommercePayment
	)]
	public class ControlFormController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		public async Task<ResultBase<ControlFormGetAllResult>> Get(
			[FromUri] ControlFormGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ControlFormGetAllArguments, ControlFormGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ControlFormGetResult>> Get(
			int id,
			[FromUri] ControlFormGetArguments arguments,
			[Injection] IQueryBaseHandler<ControlFormGetArguments, ControlFormGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /Selector/{filter?}
		[HttpGet]

		[Route("Selector/{filter?}")]
		public async Task<ResultBase<ControlFormGetSelectorResult>> Selector(
			string filter,
			[FromUri] ControlFormGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<ControlFormGetSelectorArguments, ControlFormGetSelectorResult> handler
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
			ControlFormCreateArguments command,
			[Injection] IServiceBaseHandler<ControlFormCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion POST /

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
				int id,
				ControlFormUpdateArguments arguments,
				[Injection] IServiceBaseHandler<ControlFormUpdateArguments> handler
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
				int id,
				[FromUri] ControlFormDeleteArguments command,
				[Injection] IServiceBaseHandler<ControlFormDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion DELETE /{id:int}
	}
}
