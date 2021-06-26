using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
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
	[RoutePrefix("Api/ServiceCard")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker + "," + AccountRoles.User
	)]
	public class ServiceCardController : ApiController
	{
		#region GET /{filter?}
		[HttpGet]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.User
		)]
		public async Task<ResultBase<ServiceCardGetAllResult>> GetAll(
			[FromUri] ServiceCardGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ServiceCardGetAllArguments, ServiceCardGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{filter?}

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.User
		)]
		public async Task<ResultBase<ServiceCardGetResult>> Get(
			int id,
			[FromUri] ServiceCardGetArguments arguments,
			[Injection] IQueryBaseHandler<ServiceCardGetArguments, ServiceCardGetResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /MyCards/{filter?}
		[HttpGet]
		[Route("MyCards")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.User
		)]
		public async Task<ResultBase<MobileServiceCardGetAllResult>> GetAllMyCards(
			[FromUri] MobileServiceCardGetAllArguments arguments,
			[Injection] IQueryBaseHandler<MobileServiceCardGetAllArguments, MobileServiceCardGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /MyCards/{filter?}

		#region POST /Create
		[HttpPost]
		[Route("Create")]
		public async Task<dynamic> Create(
			ServiceCardCreateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceCardCreateArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /Create

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Delete(
			int id,
			[FromUri] ServiceCardDeleteArguments command,
			[Injection] IServiceBaseHandler<ServiceCardDeleteArguments> handler
			)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion DELETE /{id:int}

		#region PUT /Destroy/{id:int}
		[HttpPut]
		[Route("Destroy/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Destroy(
			int id,
			[FromUri] ServiceCardDestroyArguments command,
			[Injection] IServiceBaseHandler<ServiceCardDestroyArguments> handler
			)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion PUT /Destroy/{id:int}

		#region PUT /LockCard/{id:int}
		[HttpPut]
		[Route("LockCard/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.User
		)]
		public async Task<dynamic> LockCard(
			[FromUri] ServiceCardLockArguments command,
			[Injection] IServiceBaseHandler<ServiceCardLockArguments> handler
			)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion PUT /LockCard/{id:int}

		#region PUT /UnlockCard/{id:int}
		[HttpPut]
		[Route("UnlockCard/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.User
		)]
		public async Task<dynamic> UnlockCard(
			[FromUri] ServiceCardUnlockArguments command,
			[Injection] IServiceBaseHandler<ServiceCardUnlockArguments> handler
			)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion PUT /UnlockCard/{id:int}

		#region GET /RetrieveSelector/{filter}
		[HttpGet]
		[Route("RetrieveSelector/{filter}")]
		public async Task<ResultBase<SelectorResult>> RetrieveSelector(
			[FromUri] ServiceCardGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<ServiceCardGetSelectorArguments, SelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /RetrieveSelector/{filter}	}

		#region PUT /Link
		[HttpPut]
		[Route("Link")]
		public async Task<dynamic> Link(
			MobileServiceCardLinkArguments arguments,
			[Injection] IServiceBaseHandler<MobileServiceCardLinkArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /Link

		#region PUT /LinkCard/{id:int}
		[HttpPut]
		[Route("LinkCard/{id:int}")]
		public async Task<dynamic> LinkCard(
			int id,
			ServiceCardLinkCardArguments arguments,
			[Injection] IServiceBaseHandler<ServiceCardLinkCardArguments> handler
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /LinkCard/{id:int}

		#region PUT /Unlink/{id:int}
		[HttpPut]
		[Route("Unlink/{id:int}")]
		public async Task<dynamic> Unlink(
			int id,
			MobileServiceCardUnlinkArguments arguments,
			[Injection] IServiceBaseHandler<MobileServiceCardUnlinkArguments> handler
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
        #endregion PUT /Unlink/{id:int}
    }
}
