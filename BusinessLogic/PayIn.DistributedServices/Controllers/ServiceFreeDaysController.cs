using PayIn.Application.Dto.Arguments.ServiceFreeDays;
using PayIn.Application.Dto.Results.ServiceFreeDays;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
	public class ServiceFreeDaysController : ApiController
	{
		#region GET /
		public async Task<ResultBase<ServiceFreeDaysGetAllByConcessionResult>> Get(
			[FromUri] ServiceFreeDaysGetAllArguments command,
			[Injection] IQueryBaseHandler<ServiceFreeDaysGetAllArguments, ServiceFreeDaysGetAllByConcessionResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /

		#region Get By Id /
		[HttpGet]
		public async Task<ResultBase<ServiceFreeDaysGetByIdResult>> Get(
		int id,
		[FromUri] ServiceFreeDaysGetByIdArguments query,
		[Injection] IQueryBaseHandler<ServiceFreeDaysGetByIdArguments, ServiceFreeDaysGetByIdResult> handler
		)
		{
						query.Id = id;
						return await handler.ExecuteAsync(query);
		}
		#endregion

		#region POST /
		[HttpPost]
		public async Task<dynamic> Post(
			ServiceFreeDaysCreateArguments command,
			[Injection] IServiceBaseHandler<ServiceFreeDaysCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { Id = item.Id };
		}
		#endregion POST /

		#region PUT /
		[HttpPut]
		public async Task<dynamic> Put(
			int id,
			ServiceFreeDaysUpdateArguments command,
			[Injection] IServiceBaseHandler<ServiceFreeDaysUpdateArguments> handler
		)
		{
			command.Id = id;
			var item = await handler.ExecuteAsync(command);
			return new { Id = item.Id };
		}
		#endregion PUT /

		#region DELETE /
		[HttpDelete]
		public async Task<dynamic> Delete(
			int id,
			[FromUri] ServiceFreeDaysDeleteArguments command,
			[Injection] IServiceBaseHandler<ServiceFreeDaysDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion DELETE /
	}
}
