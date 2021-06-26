using System.Threading.Tasks;
using System.Web.Http;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/ServiceCategory")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles =
			AccountRoles.Superadministrator + "," +
			AccountRoles.Operator + "," +
			AccountRoles.PaymentWorker + "," +
			AccountRoles.Commerce + "," +
			AccountRoles.CommercePayment
	)]
	public class ServiceCategoryController : ApiController
	{
		#region GET /{filter?}
		[HttpGet]
		[Route("{filter?}")]
		public async Task<ResultBase<ServiceCategoryGetAllResult>> GetAll(
			[FromUri] ServiceCategoryGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ServiceCategoryGetAllArguments, ServiceCategoryGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{filter?}
		
		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ServiceCategoryGetResult>> Get(
			[FromUri] ServiceCategoryGetArguments arguments,
			[Injection] IQueryBaseHandler<ServiceCategoryGetArguments, ServiceCategoryGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}
		
		#region POST /
		[HttpPost]
		[Route()]
		public async Task<dynamic> Post(
			[FromBody] ServiceCategoryCreateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceCategoryCreateArguments> handler
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
			ServiceCategoryUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceCategoryUpdateArguments> handler
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
			[FromUri] ServiceCategoryDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ServiceCategoryDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}
		
	}
}
