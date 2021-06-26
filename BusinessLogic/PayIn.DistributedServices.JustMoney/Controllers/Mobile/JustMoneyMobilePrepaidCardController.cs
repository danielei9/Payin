using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.Dto.JustMoney.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.JustMoney.Controllers.Mobile
{
	[HideSwagger]
	[RoutePrefix("JustMoney/Mobile/PrepaidCard/v1")]
	[XpAuthorize(
		ClientIds = AccountClientId.JustMoney,
		Roles = AccountRoles.User
	)]
	public class JustMoneyMobilePrepaidCardController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		public async Task<dynamic> Get(
			[FromUri] JustMoneyApiPrepaidCardGetAllArguments arguments,
			[Injection] IQueryBaseHandler<JustMoneyApiPrepaidCardGetAllArguments, JustMoneyApiPrepaidCardGetAllResult> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion GET /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<dynamic> Get(
			int id,
			[FromUri] JustMoneyMobilePrepaidCardGetArguments arguments,
			[Injection] IQueryBaseHandler<JustMoneyMobilePrepaidCardGetArguments, JustMoneyApiPrepaidCardGetAllResult> handler
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion GET /{id:int}

		#region GET /Log/{id:int}
		[HttpGet]
		[Route("Log/{id:int}")]
		public async Task<dynamic> Log(
			int id,
			[FromUri] JustMoneyMobilePrepaidCardGetLogArguments arguments,
			[Injection] IQueryBaseHandler<JustMoneyMobilePrepaidCardGetLogArguments, JustMoneyMobilePrepaidCardGetLogResult> handler
		)
		{
			arguments.Id = id;

			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion GET /Log/{id:int}

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

		#region POST /AddCard
		[HttpPost]
		[Route("AddCard")]
		public async Task<dynamic> AddCard(
			JustMoneyApiPrepaidCardCreateCardArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyApiPrepaidCardCreateCardArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion POST /AddCard

		#region POST /ShareFunds
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
		#endregion POST /ShareFunds

		#region PUT /Enable/{id:int}
		[HttpPut]
		[Route("Enable/{id:int}")]
		public async Task<dynamic> Enable(
			int id,
			[FromBody] JustMoneyMobilePrepaidCardEnableArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyMobilePrepaidCardEnableArguments> handler
		)
		{
			arguments.Id = id;

			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion PUT /Enable/{id:int}

		#region PUT /Details/{id:int}
		[HttpPut]
		[Route("Details/{id:int}")]
		public async Task<dynamic> PutDetails(
			int id,
			JustMoneyMobilePrepaidCardPutDetailsArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyMobilePrepaidCardPutDetailsArguments> handler
		)
		{
			arguments.Id = id;

			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion PUT /DetailsI{id:int}

		#region PUT /Disable/{id:int}
		[HttpPut]
		[Route("Disable/{id:int}")]
		public async Task<dynamic> Disable(
			int id,
			[FromBody] JustMoneyMobilePrepaidCardDisableArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyMobilePrepaidCardDisableArguments> handler
		)
		{
			arguments.Id = id;

			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion PUT /Disable/{id:int}
	}
}
