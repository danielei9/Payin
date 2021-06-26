using PayIn.Application.Dto.Arguments.ServiceSupplier;
using PayIn.Application.Dto.Results.ServiceSupplier;
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
	[RoutePrefix("Api/ServiceSupplier")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.Operator + "," + AccountRoles.User
	)]
	public class ServiceSupplierController : ApiController
	{
		#region GET /Selector/{filter?}
		[HttpGet]
		[Route("Selector/{filter?}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.Operator
		)]
		public async Task<ResultBase<ServiceSupplierGetSelectorResult>> Selector(
			string filter,
			[FromUri] ServiceSupplierGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<ServiceSupplierGetSelectorArguments, ServiceSupplierGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Selector/{filter?}

		#region GET /
		[HttpGet]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator
		)]
		public async Task<ResultBase<ServiceSupplierGetAllResult>> GetAll(
		    [FromUri] ServiceSupplierGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ServiceSupplierGetAllArguments, ServiceSupplierGetAllResult> handler
		)
        {
			var result = await handler.ExecuteAsync(arguments);
			return result;			
	    }
		#endregion GET /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.Operator
		)]
		public async Task<ResultBase<ServiceSupplierGetResult>> Get(	
		    [FromUri] ServiceSupplierGetArguments argument,
			[Injection] IQueryBaseHandler<ServiceSupplierGetArguments, ServiceSupplierGetResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /Current
		[HttpGet]
		[Route("Current")]
		public async Task<ResultBase<ServiceSupplierGetCurrentResult>> Get(
			[FromUri] ServiceSupplierGetCurrentArguments argument,
			[Injection] IQueryBaseHandler<ServiceSupplierGetCurrentArguments, ServiceSupplierGetCurrentResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /{id:int}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.Operator
		)]
		public async Task<dynamic> Put(int id,
			ServiceSupplierUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceSupplierUpdateArguments> handler
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /{id:int}
	}
}
