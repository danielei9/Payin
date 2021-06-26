using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
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
	[RoutePrefix("Api/EntranceType")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.User
	)]
	public class ApiEntranceTypeController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.Superadministrator
		)]
		public async Task<ResultBase<EntranceTypeGetAllResult>> GetAll(
			[FromUri] EntranceTypeGetAllArguments arguments,
			[Injection] IQueryBaseHandler<EntranceTypeGetAllArguments, EntranceTypeGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /
		
		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<EntranceTypeGetResult>> Get(
            int id,
			[FromUri] EntranceTypeGetArguments arguments,
			[Injection] IQueryBaseHandler<EntranceTypeGetArguments, EntranceTypeGetResult> handler)
		{
            arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region POST /AddGroup/{id:int}	
		[HttpPost]
		[Route("AddGroup/{id:int}")]
		public async Task<dynamic> AddGroup(
			int id,
			EntranceTypeAddGroupArguments arguments,
			[Injection] IServiceBaseHandler<EntranceTypeAddGroupArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /AddGroup/{id:int}

		#region PUT /RemoveGroup/{id:int}	
		[HttpPut]
		[Route("RemoveGroup/{id:int}")]
		public async Task<dynamic> RemoveGroup(
			int id,
			EntranceTypeRemoveGroupArguments arguments,
			[Injection] IServiceBaseHandler<EntranceTypeRemoveGroupArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return null;
		}
		#endregion PUT /RemoveGroup/{id:int}

		#region GET /Groups/{id:int}
		[HttpGet]
		[Route("Groups/{id:int}")]
		public async Task<ResultBase<EntranceTypeGroupsGetAllResult>> GetGroups(
			int id,
			[FromUri] EntranceTypeGroupsGetAllArguments argument,
			[Injection] IQueryBaseHandler<EntranceTypeGroupsGetAllArguments, EntranceTypeGroupsGetAllResult> handler)
		{
			argument.Id = id;
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /Groups/{id:int}

		#region GET /Visibility/{id:int}
		[HttpGet]
		[Route("Visibility/{id:int}")]
		public async Task<ResultBase<EntranceTypeGetVisibilityResult>> GetVisibility(
			int id,
			[FromUri] EntranceTypeGetVisibilityArguments arguments,
			[Injection] IQueryBaseHandler<EntranceTypeGetVisibilityArguments, EntranceTypeGetVisibilityResult> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Visibility/{id:int}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment)]
		public async Task<dynamic> Put(
			int id,
			EntranceTypeUpdateArguments arguments,
			[Injection] IServiceBaseHandler<EntranceTypeUpdateArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region PUT /Visibility/{id:int}
		[HttpPut]
		[Route("Visibility/{id:int}")]
		public async Task<dynamic> Visibility(
			int id,
			[FromUri] EntranceTypeVisibilityArguments arguments,
			[Injection] IServiceBaseHandler<EntranceTypeVisibilityArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /Visibility/{id:int}

		#region POST /{eventId:int}
		[HttpPost]
		[Route("{eventId:int}")]
		public async Task<dynamic> Post(
            int eventId,
			EntranceTypeCreateArguments arguments,
			[Injection] IServiceBaseHandler<EntranceTypeCreateArguments> handler
		)
		{
            arguments.EventId = eventId;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
        #endregion POST /{eventId:int}

        #region POST /ImageCrop/{id:int}
        [HttpPut]
		[Route("ImageCrop/{id:int}")]
		public async Task<dynamic> EntranceTypeImage(
			int id,
			EntranceTypeUpdatePhotoArguments arguments,
			[Injection] IServiceBaseHandler<EntranceTypeUpdatePhotoArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /ImageCrop/{id:int}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Delete(
			int id,
			[FromUri] EntranceTypeDeleteArguments command,
			[Injection] IServiceBaseHandler<EntranceTypeDeleteArguments> handler
			)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
        #endregion DELETE /{id:int}

        #region PUT /Relocate /{id:int}
        [HttpPut]
        [Route("Relocate/{id:int}")]
        [XpAuthorize(Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment)]
        public async Task<dynamic> Relocate(
            int id,
            EntranceTypeRelocateArguments arguments,
            [Injection] IServiceBaseHandler<EntranceTypeRelocateArguments> handler)
        {
            arguments.OldId = id;
            var item = await handler.ExecuteAsync(arguments);
            return null;
        }
		#endregion PUT /Relocate /{id:int}

		#region GET /RetrieveSelector
		[HttpGet]
        [Route("RetrieveSelector/{filter?}")]
        public async Task<ResultBase<EntranceTypeGetSelectorResult>> RetrieveSelector(
			string filter,
            [FromUri] EntranceTypeGetSelectorArguments argument,
            [Injection] IQueryBaseHandler<EntranceTypeGetSelectorArguments, EntranceTypeGetSelectorResult> handler
        )
        {
            var result = await handler.ExecuteAsync(argument);
            return result;
        }
		#endregion GET /RetrieveSelector

		#region PUT /IsVisible/{id:int}
		[HttpPut]
		[Route("IsVisible/{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker)]
		public async Task<dynamic> IsVisible(
			int id,
			EntranceTypeIsVisibleArguments arguments,
			[Injection] IServiceBaseHandler<EntranceTypeIsVisibleArguments> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /IsVisible/{id:int}

		#region GET /GetSellable
		[HttpGet]
		[Route("GetSellable")]
		public async Task<ResultBase<MobileEntranceTypeGetSellableResult>> GetSellable(
			//int id,
			[FromUri] MobileEntranceTypeGetSellableArguments argument,
			[Injection] IQueryBaseHandler<MobileEntranceTypeGetSellableArguments, MobileEntranceTypeGetSellableResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
        #endregion GET /GetSellable

        #region GET /GetBuyable
        [HttpGet]
        [Route("GetBuyable")]
        public async Task<ResultBase<MobileEntranceTypeGetBuyableResult>> GetBuyable(
            [FromUri] MobileEntranceTypeGetBuyableArguments arguments,
            [Injection] IQueryBaseHandler<MobileEntranceTypeGetBuyableArguments, MobileEntranceTypeGetBuyableResult> handler)
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /GetBuyable

        #region GET /GetToGive
        [HttpGet]
		[Route("GetToGive")]
		public async Task<ResultBase<MobileEntranceTypeGetToGiveResult>> GetToGive(
			//int id,
			[FromUri] MobileEntranceTypeGetToGiveArguments argument,
			[Injection] IQueryBaseHandler<MobileEntranceTypeGetToGiveArguments, MobileEntranceTypeGetToGiveResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /GetToGive

		#region GET /GetPurses
		[HttpGet]
		[Route("GetPurses")]
		public async Task<ResultBase<MobileEntranceTypeGetPursesResult>> GetPurses(
			[FromUri] MobileEntranceTypeGetPursesArguments argument,
			[Injection] IQueryBaseHandler<MobileEntranceTypeGetPursesArguments, MobileEntranceTypeGetPursesResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /GetPurses

		#region GET /GetEntranceTypeBuyBalance
		[HttpGet]
		[Route("GetEntranceTypeBuyBalance")]
		public async Task<ResultBase<EntranceTypeBuyBalanceGetResult>> GetEntranceTypeBuyBalance(
			//int id,
			[FromUri] EntranceTypeBuyBalanceGetArguments argument,
			[Injection] IQueryBaseHandler<EntranceTypeBuyBalanceGetArguments, EntranceTypeBuyBalanceGetResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /GetEntranceTypeBuyBalance

		#region GET /GetEntranceTypeRecharge
		[HttpGet]
		[Route("GetEntranceTypeRecharge")]
		public async Task<ResultBase<EntranceTypeRechargeGetResult>> GetEntranceTypeRecharge(
			//int id,
			[FromUri] EntranceTypeRechargeGetArguments argument,
			[Injection] IQueryBaseHandler<EntranceTypeRechargeGetArguments, EntranceTypeRechargeGetResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /GetEntranceTypeRecharge

		#region GET /GetEntranceTypeDonate
		[HttpGet]
		[Route("GetEntranceTypeDonate")]
		public async Task<ResultBase<EntranceTypeDonateGetResult>> GetEntranceTypeDonate(
			//int id,
			[FromUri] EntranceTypeDonateGetArguments argument,
			[Injection] IQueryBaseHandler<EntranceTypeDonateGetArguments, EntranceTypeDonateGetResult> handler)
		{
			//argument.Id = id;
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /GetEntranceTypeDonate
	}
}
