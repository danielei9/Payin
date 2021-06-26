using System.Web.Http;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.Dto.JustMoney.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;
using System;

namespace PayIn.DistributedServices.JustMoney.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("JustMoney/Api/PrepaidCard")]
	public class JustMoneyApiPrepaidCardController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> Get(
			[FromUri] JustMoneyApiPrepaidCardGetAllArguments arguments,
			[Injection] IQueryBaseHandler<JustMoneyApiPrepaidCardGetAllArguments, JustMoneyApiPrepaidCardGetAllResult> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion GET /

		#region GET /EnableDisable
		[HttpGet]
		[Route("EnableDisable")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> GetEnableDisable(
			[FromUri] JustMoneyApiPrepaidCardGetEnableDisableArguments arguments,
			[Injection] IQueryBaseHandler<JustMoneyApiPrepaidCardGetEnableDisableArguments, JustMoneyApiPrepaidCardGetEnableDisableResult> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion GET /EnableDisable

		#region GET /Log
		[HttpGet]
		[Route("Log")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> Log(
			[FromUri] JustMoneyApiPrepaidCardGetLogArguments arguments,
			[Injection] IQueryBaseHandler<JustMoneyApiPrepaidCardGetLogArguments, JustMoneyApiPrepaidCardGetLogResult> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion GET /Log

		#region GET /Cards
		[HttpGet]
        [Route("Cards/")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> GetCards(
            [FromUri] JustMoneyApiPrepaidCardGetCardsArguments arguments,
            [Injection] IQueryBaseHandler<JustMoneyApiPrepaidCardGetCardsArguments, JustMoneyApiPrepaidCardGetCardsResult> handler
        )
        {
            var item = await handler.ExecuteAsync(arguments);
            return item;
        }
		#endregion GET /Cards

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

		#region POST /CreateAndRegister
		[HttpPost]
		[Route("CreateAndRegister/")]
		public async Task<dynamic> CreateAndRegister(
			JustMoneyApiPrepaidCardCreateUserAndRegisterCardArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyApiPrepaidCardCreateUserAndRegisterCardArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion POST /Register

		//#region POST /CreateUserRegisterCard
		//[HttpPost]
		//[Route("CreateUserRegisterCard/")]
		//public async Task<dynamic> Register(
		//	JustMoneyApiPrepaidCardCreateUserRegisterCardArguments arguments,
		//	[Injection] IServiceBaseHandler<JustMoneyApiPrepaidCardCreateUserRegisterCardArguments> handler
		//)
		//{
		//	var item = await handler.ExecuteAsync(arguments);
		//	return item;
		//}
		//#endregion POST /CreateUserRegisterCard

		#region POST /CreateCard
		[HttpPost]
		[Route("CreateCard/")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> CreateCard(
			JustMoneyApiPrepaidCardCreateCardArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyApiPrepaidCardCreateCardArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion POST /CreateCard

		#region POST /RegisterCard
		[HttpPost]
		[Route("RegisterCard/")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> RegisterCard(
			JustMoneyApiPrepaidCardRegisterCardArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyApiPrepaidCardRegisterCardArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion POST /AddCard

		#region POST /Upgrade/{id:int}
		[HttpPost]
		[Route("Upgrade/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> ShareFunds(
			int id,
			[FromBody] JustMoneyApiPrepaidCardUpgradeArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyApiPrepaidCardUpgradeArguments> handler
		)
		{
			arguments.Id = id;

			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /Upgrade/{id:int}

		#region POST /ShareFunds/{id:int}
		[HttpPost]
		[Route("ShareFunds/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> ShareFunds(
			int id,
			[FromBody] JustMoneyApiPrepaidCardShareFundsArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyApiPrepaidCardShareFundsArguments> handler
		)
		{
			arguments.Id = id;

			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /ShareFunds/{id:int}

		#region POST /RechargeCard/{id:int}
		[HttpPost]
		[Route("RechargeCard/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> RechargeCard(
			int id,
			[FromBody] JustMoneyApiPrepaidCardRechargeCardArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyApiPrepaidCardRechargeCardArguments> handler
		)
		{
			arguments.Id = id;

			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /RechargeCard/{id:int}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> Update(
			JustMoneyApiPrepaidCardUpdateArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyApiPrepaidCardUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
		#endregion PUT /{id:int}

		#region PUT /EnableDisable
		[HttpPut]
		[Route("EnableDisable")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> EnableDisable(
			JustMoneyApiPrepaidCardEnableDisableArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyApiPrepaidCardEnableDisableArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
            return null; //new { item.Id };
		}
		#endregion PUT /EnableDisable

		#region PUT /Details
		[HttpPut]
		[Route("Details")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> UpdateDetails(
			JustMoneyApiPrepaidCardPutDetailsArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyApiPrepaidCardPutDetailsArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion PUT /Details
	}
}
