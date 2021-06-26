using PayIn.Application.Dto.Transport.Arguments.TransportList;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Transport.Controllers.Mobile
{
	[HideSwagger]
	[RoutePrefix("Mobile/TransportList")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.Transport + "," + AccountRoles.User
	)]
	public class MobileTransportListController : ApiController
	{
		#region POST 
		[HttpPost]
		[Route("v1/BlackList")]
		public async Task<dynamic> Post(
				BlackListManageArguments command,
			[Injection] IServiceBaseHandler<BlackListManageArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion POST 
	}
}
