using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.Dto.JustMoney.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.JustMoney.Controllers.JustMoney
{
	[HideSwagger]
	[RoutePrefix("JustMoney/Mobile/User/v1")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class JustMoneyMobileUserController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		public async Task<dynamic> Get(
			[FromUri] PrepaidCardGetAllArguments arguments,
			[Injection] IQueryBaseHandler<PrepaidCardGetAllArguments, PrepaidCardGetAllResult> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion GET /
	}
}
