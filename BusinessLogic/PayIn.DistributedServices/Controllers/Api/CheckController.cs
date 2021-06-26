using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
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
	[RoutePrefix("Api/Check")]
	[XpAuthorize(
	ClientIds = AccountClientId.Web,
	Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
	)]

	public class CheckController : ApiController
	{
        #region GET /
        [HttpGet]
        [Route("{id:int}")]
        [XpAuthorize(
            ClientIds = AccountClientId.Web,
            Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.Superadministrator
        )]
        public async Task<ResultBase<ApiEntranceCheckGetAllResult>> GetAll(
            [FromUri] ApiEntranceCheckGetAllArguments arguments,
            [Injection] IQueryBaseHandler<ApiEntranceCheckGetAllArguments, ApiEntranceCheckGetAllResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /
    }
}
