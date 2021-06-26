using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Application.Dto.Internal.Results;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
    [HideSwagger]
	[RoutePrefix("Internal/Main")]
	public class MobileMainController : ApiController
	{
		#region GET /v1/version
		[HttpGet]
		[Route("v1/version")]
		public async Task<MainGetVersionResult> GetVersion(
			[FromUri] MainGetVersionArguments arguments,
			[Injection] IQueryBaseHandler<MainGetVersionArguments, MainGetVersionResult> handler
		)
		{
			var result = (await handler.ExecuteAsync(arguments)).Data;
			return result.FirstOrDefault();
		}
		#endregion GET /v1/version
	}
}
