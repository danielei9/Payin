using PayIn.Application.Dto.Arguments.User;
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
	[RoutePrefix("Api/User")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.SystemCardOwner
	)]
	public class UserController : ApiController
	{
	
		#region POST /
		[HttpPost]
		[Route("")]
		public async Task<dynamic> Post(
			UserCreateNotificationArguments arguments,
			[Injection] IServiceBaseHandler<UserCreateNotificationArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { Id = 1 };
		}
		#endregion POST /		
	}
}
