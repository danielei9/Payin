using PayIn.Application.Dto.Payments.Arguments.PaymentUser;
using PayIn.Application.Dto.Payments.Results.PaymentUser;
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
	[RoutePrefix("Mobile/PaymentUser")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class MobilePaymentUserController : ApiController
	{
        #region PUT /v1/AcceptAssignment/{id:int}
        [HttpPut]
		[Route("v1/AcceptAssignment/{id:int}")]
		public async Task<dynamic> AcceptAssignment(
			int id,
			PaymentUserMobileAcceptAssignmentArguments command,
			[Injection] IServiceBaseHandler<PaymentUserMobileAcceptAssignmentArguments> handler
		)
		{
			command.Id = id;
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
        #endregion PUT /v1/AcceptAssignment/{id:int}

        #region PUT /v1/RejectAssignment/{id:int}
        [HttpPut]
		[Route("v1/RejectAssignment/{id:int}")]
		public async Task<dynamic> RejectAssignment(
			int id,
			PaymentUserMobileRejectAssignmentArguments arguments,
			[Injection] IServiceBaseHandler<PaymentUserMobileRejectAssignmentArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return new { Id = result.Id };
		}
        #endregion PUT /v1/RejectAssignment/{id:int}

        #region GET /v1
        [HttpGet]
        [Route("v1")]
        public async Task<ResultBase<PaymentUserMobileGetAllResult>> Get(
            [FromUri] PaymentUserMobileGetAllArguments arguments,
			ODataQueryOptions<PaymentUserMobileGetAllResult> options,
            [Injection] IQueryBaseHandler<PaymentUserMobileGetAllArguments, PaymentUserMobileGetAllResult> handler
        )
		{
			arguments.Skip = options.Skip == null ? 0 : options.Skip.Value;
			arguments.Top = Math.Min(options.Top == null ? 100 : options.Top.Value, 100);

            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /v1

        #region PUT /v1/Block/{id:int}
        [HttpPut]
		[Route("v1/Block/{id:int}")]
		public async Task<dynamic> RejectAssignment(
			int id,
			PaymentUserMobileBlockConcessionArguments arguments,
			[Injection] IServiceBaseHandler<PaymentUserMobileBlockConcessionArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return new { Id = result.Id };
		}
        #endregion PUT /v1/Block/{id:int}

        #region PUT /v1/UnblockConcession/{id:int}
        [HttpPut]
		[Route("v1/UnblockConcession/{id:int}")]
		public async Task<dynamic> RejectAssignment(
			int id,
			PaymentUserMobileUnblockConcessionArguments arguments,
			[Injection] IServiceBaseHandler<PaymentUserMobileUnblockConcessionArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return new { Id = result.Id };
		}
		#endregion PUT /v1/UnblockConcession/{id:int}

		#region DELETE /v1/DissociateConcession
		[HttpDelete]
		[Route("v1/DissociateConcession")]
		public async Task<dynamic> DissociateConcession(
			[FromUri] PaymentUserMobileDissociateConcessionArguments arguments,
			[Injection] IServiceBaseHandler<PaymentUserMobileDissociateConcessionArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /v1/DissociateConcession
	}
}
