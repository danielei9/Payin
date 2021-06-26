using PayIn.Application.Dto.Arguments.ServiceWorker;
using PayIn.Application.Dto.Results.ServiceWorker;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Mobile
{
	[HideSwagger]
	[RoutePrefix("Mobile/Worker")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class MobileWorkerController : ApiController
	{
		#region GET /v1
		[HttpGet]
		[Route("v1")]
		public async Task<ResultBase<WorkerMobileGetAllResult>> GetAll(
			[FromUri] WorkerMobileGetAllArguments arguments,
			[Injection] IQueryBaseHandler<WorkerMobileGetAllArguments, WorkerMobileGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1

		#region PUT /v1/AcceptAssignment/{id:int}
		[HttpPut]
		[Route("v1/{id:int}")]
		public async Task<dynamic> AcceptAssignment(
			int id,
			ServiceWorkerMobileAcceptAssignmentArguments arguments,
			[Injection] IServiceBaseHandler<ServiceWorkerMobileAcceptAssignmentArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return new { Id = result.Id };
		}
		#endregion PUT /v1/AcceptAssignment/{id:int}

		#region PUT /v1/RejectAssignment/{id:int}
		[HttpPut]
		[Route("v1/RejectAssignment/{id:int}")]
		public async Task<dynamic> RejectAssignment(
			int id,
			ServiceWorkerMobileRejectAssignmentArguments arguments,
			[Injection] IServiceBaseHandler<ServiceWorkerMobileRejectAssignmentArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return new { Id = result.Id };
		}
		#endregion PUT /v1/RejectAssignment/{id:int}

	}
}
