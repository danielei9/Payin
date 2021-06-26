using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData.Query;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers.Mobile
{
	[HideSwagger]
	[RoutePrefix("Mobile/PaymentWorker")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class MobilePaymentWorkerController: ApiController
	{
		#region PUT /v1/RejectAssignment/{id:int}
		[HttpPut]
		[Route("v1/RejectAssignment/{id:int}")]
		public async Task<dynamic> RejectAssignment(
			int id,
			PaymentWorkerMobileRejectAssignmentArguments arguments,
			[Injection] IServiceBaseHandler<PaymentWorkerMobileRejectAssignmentArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return new { Id = result.Id };
		}
        #endregion PUT /v1/RejectAssignment/{id:int}

        #region PUT /v1/AcceptAssignment/{id:int}
        [HttpPut]
        [Route("v1/AcceptAssignment/{id:int}")]
        public async Task<dynamic> AcceptAssignment(
            int id,
            PaymentWorkerMobileAcceptAssignmentArguments command,
            [Injection] IServiceBaseHandler<PaymentWorkerMobileAcceptAssignmentArguments> handler
        )
        {
            command.Id = id;
            var item = await handler.ExecuteAsync(command);
            return new { item.Id };
        }
        #endregion PUT /v1/AcceptAssignment/{id:int}

        #region GET /v1
        [HttpGet]
        [Route("v1")]
        public async Task<ResultBase<PaymentWorkerMobileGetAllResult>> Get(
            [FromUri] PaymentWorkerMobileGetAllArguments arguments,
			ODataQueryOptions<PaymentWorkerMobileGetAllResult> options,
            [Injection] IQueryBaseHandler<PaymentWorkerMobileGetAllArguments, PaymentWorkerMobileGetAllResult> handler
        )
        {
			arguments.Skip = options.Skip == null ? 0 : options.Skip.Value;
			arguments.Top = Math.Min(options.Top == null ? 100 : options.Top.Value, 100);

            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /v1
    }
}
