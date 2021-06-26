using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
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
	[RoutePrefix("Api/ServiceGroup")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles =
			AccountRoles.Superadministrator + "," +
			AccountRoles.Operator + "," +
			AccountRoles.PaymentWorker + "," +
			AccountRoles.Commerce + "," +
			AccountRoles.CommercePayment
	)]
	public class ServiceGroupController : ApiController
	{
		#region GET /{filter}
		[HttpGet]
		[Route("{filter}")]
		public async Task<ResultBase<ServiceGroupGetAllResult>> GetAllByFilter(
			[FromUri] ServiceGroupGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ServiceGroupGetAllArguments, ServiceGroupGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{filter}
		
		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ServiceGroupGetResult>> Get(
			[FromUri] int id,
			[FromUri] ServiceGroupGetArguments arguments,
			[Injection] IQueryBaseHandler<ServiceGroupGetArguments, ServiceGroupGetResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /RetrieveSelector/{filter?}
		[HttpGet]
		[Route("RetrieveSelector/{filter?}")]
		public async Task<ResultBase<ServiceGroupGetSelectorResult>> RetrieveSelector(
			string filter,
			[FromUri] ServiceGroupGetSelectorArguments command,
			[Injection] IQueryBaseHandler<ServiceGroupGetSelectorArguments, ServiceGroupGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /RetrieveSelector/{filter?}

		#region POST /{id:int}
		[HttpPost]
		[Route("{id:int}")]
		public async Task<dynamic> Post(
			[FromUri] int id,
			[FromBody] ServiceGroupCreateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceGroupCreateArguments> handler
		)
		{
			arguments.ServiceCategoryId = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /{id:int}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
			[FromUri] int id,
			ServiceGroupUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceGroupUpdateArguments> handler
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
			[FromUri] ServiceGroupDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ServiceGroupDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}

		#region POST /AddUser/{id:int}
		[HttpPost]
		[Route("AddUser/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> PostAddUser(
			int id,
			ServiceUserAddServiceGroupArguments arguments,
			[Injection] IServiceBaseHandler<ServiceUserAddServiceGroupArguments> handler
		)
		{
			//arguments.ServiceUserId = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /AddUser/{id:int}
	}
}
