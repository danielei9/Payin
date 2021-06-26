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
	[RoutePrefix("Api/ServiceCardBatch")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator 
		)]
	public class ServiceCardBatchController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		public async Task<ResultBase<ServiceCardBatchGetAllResult>> GetAll(
			[FromUri] ServiceCardBatchGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ServiceCardBatchGetAllArguments, ServiceCardBatchGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region POST /Create
		[HttpPost]
		[Route("Create")]
		public async Task<dynamic> Create(
			ServiceCardBatchCreateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceCardBatchCreateArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return new
			{
				Id = result.Id
			};
		}
		#endregion POST /Create

		#region PUT /Lock/{id:int}
		[HttpPut]
		[Route("Lock/{id:int}")]
		public async Task<dynamic> Lock(
			int id,
			ServiceCardBatchLockArguments command,
		[Injection] IServiceBaseHandler<ServiceCardBatchLockArguments> handler
		)
		{
			command.Id = id;
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };

		}
		#endregion PUT /Lock/{id:int}

		#region PUT /Unlock/{id:int}
		[HttpPut]
		[Route("Unlock/{id:int}")]
		public async Task<dynamic> Unlock(
			int id,
			ServiceCardBatchUnlockArguments command,
		[Injection] IServiceBaseHandler<ServiceCardBatchUnlockArguments> handler
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
			[FromUri] ServiceCardBatchDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ServiceCardBatchDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}

		#region GET /RetrieveSelector/{filter}
		[HttpGet]
		[Route("RetrieveSelector/{filter}")]
		public async Task<ResultBase<SelectorResult>> RetrieveSelector(
			[FromUri] ServiceCardBatchGetSelectorArguments command,
			[Injection] IQueryBaseHandler<ServiceCardBatchGetSelectorArguments, SelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /RetrieveSelector/{filter}
	}
}
