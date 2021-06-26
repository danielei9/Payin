using PayIn.Application.Dto.Arguments.ServiceConcession;
using PayIn.Application.Dto.Results.ServiceConcession;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
	public class ServiceConcessionController : ApiController
	{
		#region GET /
		[HttpGet]
		public async Task<ResultBase<ServiceConcessionGetAllResult>> Get(
			[FromUri] ServiceConcessionGetAllArguments command,
			[Injection] IQueryBaseHandler<ServiceConcessionGetAllArguments, ServiceConcessionGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /

		#region POST /
		[HttpPost]
		public async Task<dynamic> Post(
			ServiceConcessionCreateArguments command,
			[Injection] IServiceBaseHandler<ServiceConcessionCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { Id = item.Id };
		}
		#endregion POST /

		#region PUT /
		[HttpPut]
		public async Task<dynamic> Put(
			ServiceConcessionUpdateArguments command,
			[Injection] IServiceBaseHandler<ServiceConcessionUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { Id = item.Id };
		}
		#endregion PUT /

		#region DELETE /
		[HttpDelete]
		public async Task<dynamic> Delete(
			[FromUri] ServiceConcessionDeleteArguments command,
			[Injection] IServiceBaseHandler<ServiceConcessionDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion DELETE /

		#region GET /RetrieveSelector
		[HttpGet]
		[Route("RetrieveSelector/{filter?}")]
		public async Task<ResultBase<ServiceConcessionGetSelectorResult>> RetrieveSelector(
			[FromUri] ServiceConcessionGetSelectorArguments command,
			[Injection] IQueryBaseHandler<ServiceConcessionGetSelectorArguments, ServiceConcessionGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /RetrieveSelector
	}
}
