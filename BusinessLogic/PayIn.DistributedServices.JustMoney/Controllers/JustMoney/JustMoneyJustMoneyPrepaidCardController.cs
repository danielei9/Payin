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
	[RoutePrefix("JustMoney/JustMoney/PrepaidCard")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.User
	)]
	public class JustMoneyJustMoneyPrepaidCardController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		public async Task<dynamic> Get(
			[FromUri] JustMoneyJustMoneyPrepaidCardGetAllArguments arguments,
			[Injection] IQueryBaseHandler<JustMoneyJustMoneyPrepaidCardGetAllArguments, JustMoneyJustMoneyPrepaidCardGetAllResult> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
        #endregion GET /

        #region POST /
        [HttpPost]
		[Route]
		public async Task<dynamic> Create(
			JustMoneyApiPrepaidCardCreateUserAndRequestCardArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyApiPrepaidCardCreateUserAndRequestCardArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion POST /

		#region PUT /
		[HttpPut]
		[Route]
		public async Task<dynamic> Update(
			[FromBody] JustMoneyJustMoneyPrepaidCardUpdateArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyJustMoneyPrepaidCardUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion PUT /{id:int}

		#region PUT /Enable
		[HttpPut]
		[Route("Enable")]
		public async Task<dynamic> Enable(
			[FromBody] JustMoneyJustMoneyPrepaidCardEnableArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyJustMoneyPrepaidCardEnableArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion PUT /Enable

		#region PUT /Disable
		[HttpPut]
		[Route("Disable")]
		public async Task<dynamic> Disable(
			[FromBody] JustMoneyJustMoneyPrepaidCardDisableArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyJustMoneyPrepaidCardDisableArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion PUT /Disable

		#region PUT /Lost
		[HttpPut]
		[Route("Lost")]
		public async Task<dynamic> Lost(
			[FromBody] JustMoneyJustMoneyPrepaidCardLostArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyJustMoneyPrepaidCardLostArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion PUT /Lost
	}
}
