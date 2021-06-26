using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Mobile/Activity")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class MobileActivityController : ApiController
	{
        #region GET /v1/Event/{id:int}
        [HttpGet]
		[Route("v1/Event/{id:int}")]
		public async Task<ResultBase<MobileActivityGetByEventResult>> GetAll(
			[FromUri] MobileActivityGetByEventArguments arguments,
			[Injection] IQueryBaseHandler<MobileActivityGetByEventArguments, MobileActivityGetByEventResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /v1/Event/{id:int}
    }
}
