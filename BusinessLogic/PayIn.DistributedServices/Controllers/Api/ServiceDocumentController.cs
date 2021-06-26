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
	[RoutePrefix("Api/ServiceDocument")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.User
	)]
	public class ServiceDocumentController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		public async Task<ResultBase<ServiceDocumentGetAllResult>> GetAll(
			[FromUri] ServiceDocumentGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ServiceDocumentGetAllArguments, ServiceDocumentGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /GetCreate
		[HttpGet]
		[Route("GetCreate")]
		public async Task<ResultBase<ServiceDocumentGetCreateResult>> GetCreate(
			[FromUri] ServiceDocumentGetCreateArguments arguments,
			[Injection] IQueryBaseHandler<ServiceDocumentGetCreateArguments, ServiceDocumentGetCreateResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /GetCreate

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ServiceDocumentGetResult>> Get(
			[FromUri] int id,
			[FromUri] ServiceDocumentGetArguments arguments,
			[Injection] IQueryBaseHandler<ServiceDocumentGetArguments, ServiceDocumentGetResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region POST /
		[HttpPost]
		[Route]
		public async Task<dynamic> Post(
			[FromBody] ServiceDocumentCreateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceDocumentCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /{id:int}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
			[FromUri] int id,
			[FromBody]ServiceDocumentUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceDocumentUpdateArguments> handler
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
			[FromUri] int id,
			[FromUri] ServiceDocumentDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ServiceDocumentDeleteArguments> handler
		)
		{
			arguments.Id = id;

			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}
	}
}
