using PayIn.Application.Dto.Arguments.ServiceNumberPlate;
using PayIn.Application.Dto.Queries;
using PayIn.Application.Dto.Results;
using PayIn.Application.Dto.Results.ServiceNumberPlate;
using PayIn.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
	public class ServiceNumberPlateController : ApiController
	{
		#region GET /
		public async Task<ResultBase<ServiceNumberPlateGetAllResult>> Get(
			[FromUri] ServiceNumberPlateGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ServiceNumberPlateGetAllArguments, ServiceNumberPlateGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region POST /
		public async Task<dynamic> Post(
			ServiceNumberPlateCreateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceNumberPlateCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { Id = item.Id };
		}
		#endregion POST /

		#region PUT /
		public async Task<dynamic> Put(
			int id,
			ServiceNumberPlateUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceNumberPlateUpdateArguments> handler
		)
		{
			arguments.Id = id;

			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /

		#region DELETE /
		public async Task<dynamic> Delete(
			int id,
			[FromUri] ServiceNumberPlateDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ServiceNumberPlateDeleteArguments> handler
		)
		{
			arguments.Id = id;

			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /
	}
}
