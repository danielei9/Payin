using PayIn.Application.Dto.Arguments.ControlFormArgument;
using PayIn.Application.Dto.Results.ControlFormArgument;
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
	[RoutePrefix("Api/ControlFormArgument")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Operator + "," + AccountRoles.PaymentWorker + "," + AccountRoles.CommercePayment
	)]
	public class ControlFormArgumentController : ApiController
	{
		#region GET /Form/{formId}
		[HttpGet]
		[Route("Form")]
		public async Task<ResultBase<ControlFormArgumentGetFormResult>> GetForm(
			int formId,
			[FromUri] ControlFormArgumentGetFormArguments arguments,
			[Injection] IQueryBaseHandler<ControlFormArgumentGetFormArguments, ControlFormArgumentGetFormResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Form/{formId}

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ControlFormArgumentGetResult>> Get(
			int id,
			[FromUri] ControlFormArgumentGetArguments arguments,
			[Injection] IQueryBaseHandler<ControlFormArgumentGetArguments, ControlFormArgumentGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region POST /{formId?}
		[HttpPost]
		[Route]
		public async Task<dynamic> Post(
			ControlFormArgumentCreateArguments command,
			[Injection] IServiceBaseHandler<ControlFormArgumentCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion POST /{formId?}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
			int id,
			ControlFormArgumentUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ControlFormArgumentUpdateArguments> handler
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
				[FromUri] ControlFormArgumentDeleteArguments command,
				[Injection] IServiceBaseHandler<ControlFormArgumentDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion DELETE /{id:int}
	}
}
