using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Web.Http;
using Xp.DistributedServices.Filters;

namespace PayIn.DistributedServices.Transport.Controllers.Mobile
{
	[HideSwagger]
	[RoutePrefix("Mobile/TransportList")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.Transport + "," + AccountRoles.User
	)]
	public class MobileTransportTitleController : ApiController
	{
	}
}
