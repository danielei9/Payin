using PayIn.Application.Dto.Tsm.Arguments;
using PayIn.Application.Dto.Tsm.Results;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Tsm.Controllers
{
	[RoutePrefix("Tsm/v1")]
	//[XpAuthorize(
	//	ClientIds = AccountClientId.Web,
	//	Roles = AccountRoles.Transport + "," + AccountRoles.TransportOperator + "," + AccountRoles.Superadministrator
	//)]
	public class TsmController : ApiController
	{
		#region GET /Personalize
		[HttpGet]
		[Route("Personalize")]
		public async Task<ResultBase<TsmPersonalizeResults>> Personalize(
			[FromUri] TsmPersonalizeArguments arguments,
			[Injection] IQueryBaseHandler<TsmPersonalizeArguments, TsmPersonalizeResults> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Personalize
	}
}
