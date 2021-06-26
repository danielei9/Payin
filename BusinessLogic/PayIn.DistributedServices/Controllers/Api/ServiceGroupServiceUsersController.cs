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
	[RoutePrefix("api/servicegroupserviceusers")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles =
			AccountRoles.Superadministrator + "," +
			AccountRoles.Operator + "," +
			AccountRoles.PaymentWorker + "," +
			AccountRoles.Commerce + "," +
			AccountRoles.CommercePayment
	)]
	public class ServiceGroupServiceUsersController : ApiController
	{
		#region GET /{filter?}
		[HttpGet]
		[Route("{filter?}")]
		public async Task<ResultBase<ServiceGroupServiceUsersGetAllResult>> Get(
			[FromUri] ServiceGroupServiceUsersGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ServiceGroupServiceUsersGetAllArguments, ServiceGroupServiceUsersGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{filter?}

		#region GET /RetrieveSelector/{filter?}
		[HttpGet]
		[Route("RetrieveSelector/{filter?}")]
		public async Task<ResultBase<ServiceGroupServiceUsersGetSelectorResult>> RetrieveSelector(
			string filter,
			[FromUri] ServiceGroupServiceUsersGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<ServiceGroupServiceUsersGetSelectorArguments, ServiceGroupServiceUsersGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /RetrieveSelector/{filter?}

		#region POST /
		[HttpPost]
		[Route()]
		public async Task<dynamic> Post(
			ServiceGroupServiceUsersCreateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceGroupServiceUsersCreateArguments> handler
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
			[FromUri] int id,
			ServiceGroupUpdateArguments command,
			[Injection] IServiceBaseHandler<ServiceGroupUpdateArguments> handler
		)
		{
			command.Id = id;
			var item = await handler.ExecuteAsync(command);
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
	}
}
