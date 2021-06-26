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
	[RoutePrefix("JustMoney/Mobile/Main/v1")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class JustMoneyMainController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		public async Task<dynamic> GetAll(
			[FromUri] MainGetAllArguments arguments,
			[Injection] IQueryBaseHandler<MainGetAllArguments, MainGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /
	}
}
