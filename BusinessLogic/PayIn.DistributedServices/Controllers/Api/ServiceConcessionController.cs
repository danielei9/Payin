using System.Threading.Tasks;
using System.Web.Http;
using PayIn.Web.Security;
using PayIn.Domain.Security;
using PayIn.Application.Dto.Arguments.ServiceConcession;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;
using PayIn.Application.Dto.Results.ServiceConcession;
using Xp.DistributedServices.Filters;
using PayIn.Application.Dto.Arguments;

namespace PayIn.DistributedServices.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/ServiceConcession")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = 
			AccountRoles.Superadministrator + "," +
			AccountRoles.Operator + "," + 
			AccountRoles.Commerce + "," + 
			AccountRoles.CommercePayment + "," + 
			AccountRoles.User
	)]
	public class ServiceConcessionController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<ServiceConcessionGetAllCommerceResult>> GetAll(
			[FromUri] ServiceConcessionGetAllCommerceArguments arguments,
			[Injection] IQueryBaseHandler<ServiceConcessionGetAllCommerceArguments, ServiceConcessionGetAllCommerceResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /States
		[HttpGet]
		[Route("States")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<ServiceConcessionGetStateResult>> States(
			[FromUri] ServiceConcessionGetStateArguments arguments,
			[Injection] IQueryBaseHandler<ServiceConcessionGetStateArguments, ServiceConcessionGetStateResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /ConcessionState

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ServiceConcessionGetResult>> Get(
			[FromUri] ServiceConcessionGetArguments argument,
			[Injection] IQueryBaseHandler<ServiceConcessionGetArguments, ServiceConcessionGetResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /GetCommerce/{id:int}
		[HttpGet]
		[Route("GetCommerce/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment)]
		public async Task<ResultBase<ServiceConcessionGetCommerceResult>> GetCommerce(
			[FromUri] ServiceConcessionGetCommerceArguments argument,
			[Injection] IQueryBaseHandler<ServiceConcessionGetCommerceArguments, ServiceConcessionGetCommerceResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /GetCommerce/{id:int}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator)]
		public async Task<dynamic> Put(
			ServiceConcessionUpdateArguments command,
			[Injection] IServiceBaseHandler<ServiceConcessionUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };	
			
		}
		#endregion PUT /{id:int}

		#region PUT /UpdateCommerce/{id:int}
		[HttpPut]
		[Route("UpdateCommerce/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment)]
		public async Task<dynamic> PutCommerce(
			ServiceConcessionUpdateCommerceArguments command,
			[Injection] IServiceBaseHandler<ServiceConcessionUpdateCommerceArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion PUT /UpdateCommerce/{id:int}

		#region POST /UpdateState
		[HttpPost]
		[Route("UpdateState")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator
		)]
		public async Task<dynamic> Post(
		   ServiceConcessionUpdateStateArguments arguments,
		   [Injection] IServiceBaseHandler<ServiceConcessionUpdateStateArguments> handler
		)
		{
			var result = (await handler.ExecuteAsync(arguments));
			return result;
		}
		#endregion POST /UpdateState

		#region GET /RetrieveSelector/{filter?}
		[HttpGet]
		[Route("RetrieveSelector/{filter?}")]
		public async Task<ResultBase<ServiceConcessionGetSelectorResult>> RetrieveSelector(
			string filter,
			[FromUri] ServiceConcessionGetSelectorArguments command,
			[Injection] IQueryBaseHandler<ServiceConcessionGetSelectorArguments, ServiceConcessionGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /RetrieveSelector/{filter?}

		#region GET /RetrieveSelectorMembers/{filter?}
		[HttpGet]
		[Route("RetrieveSelectorMembers/{filter?}")]
		public async Task<ResultBase<ServiceConcessionGetSelectorMembersResult>> RetrieveSelectorMembers(
			string filter,
			[FromUri] ServiceConcessionGetSelectorMembersArguments command,
			[Injection] IQueryBaseHandler<ServiceConcessionGetSelectorMembersArguments, ServiceConcessionGetSelectorMembersResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /RetrieveSelectorMembers/{filter?}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Delete(
				int id,
				[FromUri] ServiceConcessionDeleteArguments command,
				[Injection] IServiceBaseHandler<ServiceConcessionDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion DELETE /{id:int}
	}
}
