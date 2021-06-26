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
	[RoutePrefix("Api/ServiceUser")]
	public class ServiceUserController : ApiController
	{
		#region GET /{filter?}
		[HttpGet]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorkerCash
		)]
		public async Task<ResultBase<ServiceUserGetAllResult>> GetAll(
			[FromUri] ServiceUserGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ServiceUserGetAllArguments, ServiceUserGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{filter?}

		#region GET /DashBoard/
		[HttpGet]
		[Route("Dashboard")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<ServiceUserGetAllByDashBoardResult>> DashBoard(
			[FromUri] ServiceUserGetAllByDashBoardArguments arguments,
			[Injection] IQueryBaseHandler<ServiceUserGetAllByDashBoardArguments, ServiceUserGetAllByDashBoardResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /DashBoard/{filter?}

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<ServiceUserGetResult>> Get(
			[FromUri] ServiceUserGetArguments argument,
			[Injection] IQueryBaseHandler<ServiceUserGetArguments, ServiceUserGetResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /CreateCard
		[HttpGet]
		[Route("CreateCard")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<ServiceUserCreateCardGetResult>> GetCreateCard(
			[FromUri] ServiceUserCreateCardGetArguments argument,
			[Injection] IQueryBaseHandler<ServiceUserCreateCardGetArguments, ServiceUserCreateCardGetResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /CreateCard
		
		#region GET /CreateCardSelect
		[HttpGet]
		[Route("CreateCardSelect")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<ServiceUserCreateCardSelectGetResult>> CreateCardSelectGet(
			[FromUri] ServiceUserCreateCardSelectGetArguments argument,
			[Injection] IQueryBaseHandler<ServiceUserCreateCardSelectGetArguments, ServiceUserCreateCardSelectGetResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /CreateCardSelect

		#region GET /UpdateCard/{id:int}
		[HttpGet]
		[Route("UpdateCard/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<ServiceUserUpdateCardGetResult>> GetUpdateCard(
			[FromUri] ServiceUserUpdateCardGetArguments argument,
			[Injection] IQueryBaseHandler<ServiceUserUpdateCardGetArguments, ServiceUserUpdateCardGetResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /UpdateCard/{id:int}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Put(
			ServiceUserUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceUserUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region PUT /UpdateCard/{id:int}
		[HttpPut]
		[Route("UpdateCard/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.CommercePayment
		)]
		public async Task<dynamic> PutUpdateCard(
			int id,
			ServiceUserUpdateCardArguments arguments,
			[Injection] IServiceBaseHandler<ServiceUserUpdateCardArguments> handler
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };

		}
		#endregion PUT /UpdateCard/{id:int}

		#region POST /Create
		[HttpPost]
		[Route("Create")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Post(
			ServiceUserCreateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceUserCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /Create

		#region PUT /ImageCrop		
		[HttpPut]
		[Route("ImageCrop/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> ServiceUserImage(ServiceUserUpdatePhotoArguments arguments, [Injection] IServiceBaseHandler<ServiceUserUpdatePhotoArguments> handler)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT api/Account/ImageCrop

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Delete(
			int id,
			[FromUri] ServiceUserDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ServiceUserDeleteArguments> handler
			)
			{
				var result = await handler.ExecuteAsync(arguments);
				return result;
			}
		#endregion DELETE /{id:int}

		#region PUT /register/{id:int}
		[HttpPut]
		[Route("register/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Register(
			int id,
			[FromUri] ServiceUserRegisterArguments arguments,
			[Injection] IServiceBaseHandler<ServiceUserRegisterArguments> handler
			)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /register/{id:int}

		#region PUT /subscribe/{id:int}
		[HttpPut]
		[Route("subscribe/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Subscribe(
			int id,
			[FromUri] ServiceUserSubscribeArguments arguments,
			[Injection] IServiceBaseHandler<ServiceUserSubscribeArguments> handler
			)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /subscribe/{id:int}

		#region PUT /unsubscribe/{id:int}
		[HttpPut]
		[Route("unsubscribe/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Unsubscribe(
			int id,
			[FromUri] ServiceUserUnsubscribeArguments arguments,
			[Injection] IServiceBaseHandler<ServiceUserUnsubscribeArguments> handler
			)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /unsubscribe/{id:int}

		#region GET /RetrieveSelector/{filter}
		[HttpGet]
		[Route("RetrieveSelector/{filter}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker + "," + AccountRoles.PaymentWorker
		)]
		public async Task<ResultBase<ServiceUserGetSelectorResult>> RetrieveSelector(
			[FromUri] ServiceUserGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<ServiceUserGetSelectorArguments, ServiceUserGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /RetrieveSelector/{filter}

		#region POST /AddServiceGroup/{id:int}
		[HttpPost]
		[Route("AddServiceGroup/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> PostAddServiceGroup(
			int id,
			ServiceUserAddServiceGroupArguments arguments,
			[Injection] IServiceBaseHandler<ServiceUserAddServiceGroupArguments> handler
		)
		{
			arguments.ServiceUserId = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /AddServiceGroup/{id:int}

		#region GET /ServiceGroups/{id:int}
		[HttpGet]
		[Route("ServiceGroups/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<ServiceUserGetServiceGroupsResult>> GetServiceGroups(
			int id,
			[FromUri] ServiceUserGetServiceGroupsArguments argument,
			[Injection] IQueryBaseHandler<ServiceUserGetServiceGroupsArguments, ServiceUserGetServiceGroupsResult> handler)
		{
			argument.UserId = id;
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /ServiceGroups/{id:int}

		#region PUT /RemoveServiceGroup/{id:int}
		[HttpPut]
		[Route("RemoveServiceGroup/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> RemoveServiceGroup(
			[FromUri]int id,
			[FromBody] ServiceUserRemoveServiceGroupArguments arguments,
			[Injection] IServiceBaseHandler<ServiceUserRemoveServiceGroupArguments> handler
			)
		{
			arguments.UserId = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /RemoveServiceGroup/{id:int}
	}
}
