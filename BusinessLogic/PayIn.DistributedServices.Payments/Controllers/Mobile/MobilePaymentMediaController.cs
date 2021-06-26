using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers
{
	[HideSwagger]
	[RoutePrefix("Mobile/PaymentMedia")]
	public class MobilePaymentMediaController : ApiController
	{
		#region POST v1/WebCard
		[HttpPost]
		[Route("v1/WebCard")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> CreateWebCard(
			MobilePaymentMediaCreateWebCardArguments arguments,
			[Injection] IServiceBaseHandler<MobilePaymentMediaCreateWebCardArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion POST v1/WebCard

        #region POST v1/WebCardActivate
        [HttpPost]
        [Route("v1/WebCardActivate")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> ActivateWebCard(
            MobilePaymentMediaActivateWebCardArguments arguments,
            [Injection] IServiceBaseHandler<MobilePaymentMediaActivateWebCardArguments> handler
        )
		{
			var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion POST v1/WebCardActivate

        #region POST v1/WebCardAndUser
        [HttpPost]
        [Route("v1/WebCardAndUser")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> WebCardAndUser(
            MobilePaymentMediaCreateWebCardAndUserArguments arguments,
            [Injection] IServiceBaseHandler<MobilePaymentMediaCreateWebCardAndUserArguments> handler
        )
		{
			var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion POST v1/WebCardAndUser

        #region GET v1
        [HttpGet]
		[Route("v1")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.User
		)]
		public async Task<ResultBase<MobilePaymentMediaGetAllResult>> Selector(
			[FromUri] MobilePaymentMediaGetAllArguments arguments,
			[Injection] IQueryBaseHandler<MobilePaymentMediaGetAllArguments, MobilePaymentMediaGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET v1

		#region GET v1/ByPaymentConcession/{paymentConcessionId}
		[HttpGet]
		[Route("v1/ByPaymentConcession/{paymentConcessionId}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative,
			Roles = AccountRoles.User
		)]
		public async Task<ResultBase<MobilePaymentMediaGetAllResult>> GetByPaymentConcession(
			int paymentConcessionId,
			[FromUri] MobilePaymentMediaGetByPaymentConcessionArguments arguments,
			[Injection] IQueryBaseHandler<MobilePaymentMediaGetByPaymentConcessionArguments, MobilePaymentMediaGetAllResult> handler
		)
		{
			arguments.PaymentConcessionId = paymentConcessionId;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET v1/ByPaymentConcession/{paymentConcessionId}

		#region DELETE v1/{id:int}
		[HttpDelete]
		[Route("v1/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> Delete(
			int id,
			[FromUri] MobilePaymentMediaDeleteArguments command,
			[Injection] IServiceBaseHandler<MobilePaymentMediaDeleteArguments> handler
		)
		{
			command.Id = id;
        
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion DELETE v1/{id:int}
	}
}

