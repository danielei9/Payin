using PayIn.Application.Dto.Transport.Arguments.TransportList;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Transport.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/TransportList")]
	[XpAuthorize(
	ClientIds = AccountClientId.Web,
	Roles = AccountRoles.Transport + "," + AccountRoles.TransportOperator)]
	public class ApiTransportListController : ApiController
	{
		#region POST /GreyList
		[HttpPost]
		[Route("GreyList")]
		public async Task<dynamic> GreyList(
			UpdateGreyListArguments command,
			[Injection] IServiceBaseHandler<UpdateGreyListArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return null;
		}
		#endregion POST /GreyList
	}
}
