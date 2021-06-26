using PayIn.Application.Dto.Arguments.ServiceWorker;
using PayIn.Application.Dto.Results.ServiceWorker;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
	[HideSwagger]
	[RoutePrefix("Api/ServiceWorker")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Commerce + "," + AccountRoles.Operator
	)]
	public class ServiceWorkerController : ApiController
	{
		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ServiceWorkerGetResult>> Get(
			[FromUri] ServiceWorkerGetArguments query,
			[Injection] IQueryBaseHandler<ServiceWorkerGetArguments, ServiceWorkerGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(query);
			return result;
		}
		#endregion  GET /{id:int}

		#region GET /
		[HttpGet]
		[Route("")]
		[XpAuthorize(Roles = AccountRoles.Commerce)]
		public async Task<ResultBase<ServiceWorkerGetAllResult>> Get(
			[FromUri] ServiceWorkerGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ServiceWorkerGetAllArguments, ServiceWorkerGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /Control
		[HttpGet]
		[Route("Control")]
		[XpAuthorize(Roles = AccountRoles.Operator)]
		public async Task<ResultBase<ServiceWorkerGetControlResult>> Get(
			[FromUri] ServiceWorkerGetControlArguments arguments,
			[Injection] IQueryBaseHandler<ServiceWorkerGetControlArguments, ServiceWorkerGetControlResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Control

		#region GET /Selector/{filter?}
		[HttpGet]
		[Route("Selector/{filter?}")]
		public async Task<ResultBase<ServiceWorkerGetSelectorResult>> Selector(
			string filter,
			[FromUri] ServiceWorkerGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<ServiceWorkerGetSelectorArguments, ServiceWorkerGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Selector/{filter?}

		#region POST /
		[HttpPost]
		[Route("")]
		[XpAuthorize(Roles = AccountRoles.Commerce)]
		public async Task<dynamic> Post(
			ServiceWorkerCreateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceWorkerCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Commerce)]
		public async Task<dynamic> Put(
			ServiceWorkerUpdateArguments command,
			[Injection] IServiceBaseHandler<ServiceWorkerUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Commerce)]
		public async Task<dynamic> Delete(
			[FromUri] ServiceWorkerDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ServiceWorkerDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}

		#region Post /ChangeRole
		[HttpPost]
		[Route("ChangeRole")]
		[XpAuthorize(Roles = AccountRoles.Commerce)]
		public async Task<dynamic> ChangeRole(
			ServiceWorkerChangeRoleArguments arguments,
			[Injection] IServiceBaseHandler<ServiceWorkerChangeRoleArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			if (result)
				return Ok();
			else
				return BadRequest();
		}
		#endregion Post /ChangeRole

	}
}
