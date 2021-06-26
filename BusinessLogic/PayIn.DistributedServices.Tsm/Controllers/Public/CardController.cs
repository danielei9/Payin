using PayIn.Application.Dto.Tsm.Arguments.Card;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Tsm.Controllers
{
	[HideSwagger]
	[RoutePrefix("Tsm/Card")]
	//[XpAuthorize(
	//	ClientIds = AccountClientId.Web,
	//	Roles = AccountRoles.Transport + "," + AccountRoles.TransportOperator + "," + AccountRoles.Superadministrator
	//)]
	public class CardController : ApiController
	{
		#region POST Check
		[HttpPost]
		[Route("Check")]
		public async Task<dynamic> Check(
			CheckArguments arguments,
			[Injection] IServiceBaseHandler<CheckArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return null; // new { item.Id };

		}
		#endregion POST Check
	}
}
