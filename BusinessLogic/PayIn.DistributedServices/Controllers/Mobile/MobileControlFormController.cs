using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
    [HideSwagger]
	[RoutePrefix("Mobile/ControlForm")]
    [XpAuthorize(
        ClientIds = AccountClientId.AndroidNative,
        Roles = AccountRoles.User
    )]
    public class MobileControlFormController : ApiController
	{
		#region GET /v1/{ids}
		[HttpGet]
		[Route("v1/{ids}")]
		public async Task<ResultBase<MobileControlFormGetResult>> Get(
			string ids,
			[FromUri] MobileControlFormGetArguments arguments,
			[Injection] IQueryBaseHandler<MobileControlFormGetArguments, MobileControlFormGetResult> handler
		)
		{
			arguments.Ids = ids
                .SplitString(",")
                .Select(x => Convert.ToInt32(x));

            var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/{id}
	}
}
