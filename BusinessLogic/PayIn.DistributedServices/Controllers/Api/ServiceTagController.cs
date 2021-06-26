using PayIn.Application.Dto.Arguments.ServiceTag;
using PayIn.Application.Dto.Results.ServiceTag;
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
	[RoutePrefix("Api/ServiceTag")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator
	)]
	public class ServiceTagController : ApiController
	{
		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		[Authorize(Roles = AccountRoles.Superadministrator)]
		public async Task<ResultBase<ServiceTagGetResult>> Get(
			[FromUri] ServiceTagGetArguments command,
			[Injection] IQueryBaseHandler<ServiceTagGetArguments, ServiceTagGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /{filter?}
		[HttpGet]
		[Route("{filter?}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator)]
		public async Task<ResultBase<ServiceTagGetAllResult>> Get(
			[FromUri] ServiceTagGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ServiceTagGetAllArguments, ServiceTagGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{filter?}

		#region POST /
		[HttpPost]
		[Route("")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator)]
		public async Task<dynamic> Post(
				ServiceTagCreateArguments command,
				[Injection] IServiceBaseHandler<ServiceTagCreateArguments> handler
		)
{
			var item = await handler.ExecuteAsync(command);
		return new { item.Id };
	}
		#endregion POST

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator)]
		public async Task<dynamic> Put(
			ServiceTagUpdateArguments command,
		[Injection] IServiceBaseHandler<ServiceTagUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator)]
		public async Task<dynamic> Delete(
		int id,
			[FromUri] ServiceTagDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ServiceTagDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}

		#region GET /Selector/{filter?}
		[HttpGet]
		[Route("Selector/{filter?}")]
		[XpAuthorize(Roles = AccountRoles.Operator)]
		public async Task<ResultBase<ServiceTagGetSelectorResult>> Selector(
			string filter,
			[FromUri] ServiceTagGetSelectorArguments command,
			[Injection] IQueryBaseHandler<ServiceTagGetSelectorArguments, ServiceTagGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /Selector/{filter?}
	}
}