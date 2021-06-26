using PayIn.Application.Dto.Results;
using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Dto.Transport.Results.TransportOperation;
using PayIn.Domain.Security;
using PayIn.Domain.Transport;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.Application.Results;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Transport.Controllers
{
	[HideSwagger]
	[RoutePrefix("Mobile/TransportOperation")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.FgvTsm + "," + AccountClientId.AlacantTsm, // Esto se control tb en cada método porque FGV no puede en todos
		Roles = AccountRoles.User
	)]
	public class MobileTransportOperationController : ApiController
	{
		#region POST /v1/Search
		[HttpPost]
		[Route("v1/Search")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<ServiceCardReadInfoResult>> Search(
			TransportOperationSearchArguments arguments,
			[Injection] IQueryBaseHandler<TransportOperationSearchArguments, ServiceCardReadInfoResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v1/Search

		#region POST /v1/ClassicNoCompatible
		[HttpPost]
		[Route("v1/ClassicNoCompatible")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<ServiceCardReadInfoResult>> ClassicNoCompatible(
			TransportOperationClassicNoCompatible2Arguments arguments,
			[Injection] IQueryBaseHandler<TransportOperationClassicNoCompatible2Arguments, ServiceCardReadInfoResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v1/ClassicNoCompatible

		#region GET /v1/ReadInfo
		[HttpGet]
		[Route("v1/ReadInfo")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.FgvTsm + "," + AccountClientId.AlacantTsm,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<TransportOperationGetReadInfoResult>> GetReadInfo(
			[FromUri] TransportOperationGetReadInfoArguments arguments,
			[Injection] IQueryBaseHandler<TransportOperationGetReadInfoArguments, TransportOperationGetReadInfoResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /v1/ReadInfo

        #region POST /v1/ReadInfo
        [HttpPost]
		[Route("v1/ReadInfo/{id:int}")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.FgvTsm + "," + AccountClientId.AlacantTsm,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<ServiceCardReadInfoResult>> PutReadInfo(
			int id,
			TransportOperationReadInfoArguments arguments,
			[Injection] IQueryBaseHandler<TransportOperationReadInfoArguments, ServiceCardReadInfoResult> handler
		)
		{
            arguments.OperationId = id;
            var result = await handler.ExecuteAsync(arguments);
            return result;
		}
		#endregion POST /v1/ReadInfo

		#region PUT /v1/ConfirmAndReadInfo
		[HttpPut]
		[Route("v1/ConfirmAndReadInfo/{id:int}")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.FgvTsm + "," + AccountClientId.AlacantTsm,
            Roles = AccountRoles.User
        )]
        public async Task<dynamic> PutConfirmAndReadInfo(
			int id,
			TransportOperationConfirmAndReadInfoArguments arguments,
			[Injection] IServiceBaseHandler<TransportOperationConfirmAndReadInfoArguments> handler
		)
		{
			arguments.OperationId = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /v1/ConfirmAndReadInfo

		#region POST /v1/Recharge
		[HttpPost]
		[Route("v1/Recharge/{operationId:int}")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.FgvTsm + "," + AccountClientId.AlacantTsm,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<TransportOperationGetReadInfoResult>> PutRecharge(
			int operationId,
			TransportOperationRechargeArguments arguments,
			[Injection] IServiceBaseHandler<TransportOperationRechargeArguments> handler
		)
		{
			arguments.OperationId = operationId;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v1/Recharge

		#region POST /v1/RechargeAndPay
		[HttpPost]
		[Route("v1/RechargeAndPay/{operationId:int}")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<TransportOperationGetReadInfoResult>> PutRechargeAndPay(
			int operationId,
			TransportOperationRechargeAndPayArguments arguments,
			[Injection] IServiceBaseHandler<TransportOperationRechargeAndPayArguments> handler
		)
		{
			arguments.OperationId = operationId;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v1/RechargeAndPay

		#region POST /v1/Revoke
		[HttpPost]
		[Route("v1/Revoke")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<TransportOperationRevokeResult>> Revoke(
			TransportOperationRevokeArguments arguments,
			[Injection] IQueryBaseHandler<TransportOperationRevokeArguments, TransportOperationRevokeResult> handler
		)
		{
			arguments.RechargeType = RechargeType.Revoke;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v1/Revoke

		#region PUT /v1/Confirm
		[HttpPut]
        [Route("v1/Confirm/{id:int}")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative,
            Roles = AccountRoles.User
        )]
        public async Task<dynamic> Confirm(
            int id,
           [FromUri] TransportOperationConfirmArguments arguments,
           [Injection] IServiceBaseHandler<TransportOperationConfirmArguments> handler
        )
        {
            arguments.Id = id;
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
		#endregion PUT /v1/Confirm

		#region PUT /v1/Personalize
		[HttpPut]
		[Route("v1/Personalize")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative,
            Roles = AccountRoles.User
        )]
        public async Task<dynamic> Personalize(
			int id,
		   [FromUri] TransportOperationPersonalizeArguments arguments,
		   [Injection] IServiceBaseHandler<TransportOperationPersonalizeArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
        #endregion PUT /v1/Personalize

        #region POST /v1/Detect
        [HttpPost]
        [Route("v1/detect")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.FgvTsm + "," + AccountClientId.AlacantTsm,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<Script2Result>> Detect(
            [FromBody] TransportOperationDetectArguments arguments,
            [Injection] IQueryBaseHandler<TransportOperationDetectArguments, Script2Result> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion POST /v1/Detect

        #region POST /v1/Interpret
        [HttpPost]
        [Route("v1/interpret")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.FgvTsm + "," + AccountClientId.AlacantTsm,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<Script2Result>> Interpret(
            [FromBody] TransportOperationInterpretArguments arguments,
            [Injection] IQueryBaseHandler<TransportOperationInterpretArguments, Script2Result> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion POST /v1/Interpret

		#region POST /v2/Recharge
		[HttpPost]
		[Route("v2/recharge")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.FgvTsm + "," + AccountClientId.AlacantTsm,
			Roles = AccountRoles.User
		)]
		public async Task<ResultBase<Script2Result>> PutRecharge2(
			TransportOperationRecharge2Arguments arguments,
			[Injection] IServiceBaseHandler<TransportOperationRecharge2Arguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v2/Recharge
	}
}
