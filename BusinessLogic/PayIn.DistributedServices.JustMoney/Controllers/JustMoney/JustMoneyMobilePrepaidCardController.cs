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
	[RoutePrefix("JustMoney/Api/PrepaidCard/v1")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class JustMoneyMobilePrepaidCardController : ApiController
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

		#region POST /
		[HttpPost]
		[Route()]
		public async Task<dynamic> Create(
			PrepaidCardCreateArguments arguments,
			[Injection] IServiceBaseHandler<PrepaidCardCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion POST /

		#region PUT /Activate
		[HttpPut]
		[Route("Activate")]
		public async Task<dynamic> Activate(
			[FromBody] PrepaidCardActivateArguments arguments,
			[Injection] IServiceBaseHandler<PrepaidCardActivateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion PUT /Activate

		#region PUT /Deactivate
		[HttpPut]
		[Route("Deactivate")]
		public async Task<dynamic> Deactivate(
			int id,
			[FromBody] PrepaidCardDeactivateArguments arguments,
			[Injection] IServiceBaseHandler<PrepaidCardDeactivateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion PUT /Deactivate
	}
}
