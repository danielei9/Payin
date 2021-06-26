using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/AccessControl")]
	public class ApiAccessControlController : ApiController
	{
		#region GET /

		[HttpGet]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<ApiAccessControlGetAllResult>> GetAll(
			[FromUri] ApiAccessControlGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ApiAccessControlGetAllArguments, ApiAccessControlGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}

        #endregion

        #region GET /{id:int}

        [HttpGet]
        [Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<ApiAccessControlGetResult>> Get(
            int id,
			[FromUri] ApiAccessControlGetArguments arguments,
			[Injection] IQueryBaseHandler<ApiAccessControlGetArguments, ApiAccessControlGetResult> handler)
		{
            arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}

		#endregion

		#region GET /Places

		[HttpGet]
		[Route("Places")]
		[AllowAnonymous]
		public async Task<ResultBase<ApiAccessControlGetPlacesResult>> Places(
			[FromUri] ApiAccessControlGetPlacesArguments arguments,
			[Injection] IQueryBaseHandler<ApiAccessControlGetPlacesArguments, ApiAccessControlGetPlacesResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}

		#endregion

		#region GET /Place/{id:int}

		[HttpGet]
		[Route("Place/{id:int}")]
		[AllowAnonymous]
		public async Task<ResultBase<ApiAccessControlGetPlaceResult>> Place(
			int id,
			[FromUri] ApiAccessControlGetPlaceArguments arguments,
			[Injection] IQueryBaseHandler<ApiAccessControlGetPlaceArguments, ApiAccessControlGetPlaceResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}

		#endregion

		#region GET /Place/Weather

		[HttpGet]
		[Route("Place/Weather")]
		[AllowAnonymous]
		public async Task<ResultBase<ApiAccessControlGetWeatherResult>> PlaceWeather(
			[FromUri] ApiAccessControlGetWeatherArguments arguments,
			[Injection] IQueryBaseHandler<ApiAccessControlGetWeatherArguments, ApiAccessControlGetWeatherResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}

		#endregion

		#region POST /

		[HttpPost]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Post(
			ApiAccessControlCreateArguments command,
			[Injection] IServiceBaseHandler<ApiAccessControlCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}

		#endregion

		#region POST /Entry/{id:int}

		[HttpPost]
		[Route("Entry/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.User
		)]
		public async Task<dynamic> Entry(
			int id,
			ApiAccessControlEntryCreateArguments command,
			[Injection] IServiceBaseHandler<ApiAccessControlEntryCreateArguments> handler
		)
		{
			command.AccessControlId = id;
			var item = await handler.ExecuteAsync(command);
			return new { item.Id, item.CapacityAfterEntrance, DateTime = DateTime.UtcNow };
		}

		#endregion

		#region POST /Entry/Reset/{id:int}

		[HttpPost]
		[Route("Entry/Reset/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.User
		)]
		public async Task<dynamic> EntryReset(
			int id,
			ApiAccessControlEntryResetArguments command,
			[Injection] IServiceBaseHandler<ApiAccessControlEntryResetArguments> handler
		)
		{
			command.AccessControlId = id;
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}

		#endregion

		#region PUT /{id:int}

		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Put(
			int id,
			ApiAccessControlUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ApiAccessControlUpdateArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}

		#endregion

		#region DELETE /{id:int}

		[HttpDelete]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Delete(
			int id,
			[FromUri] ApiAccessControlDeleteArguments command,
			[Injection] IServiceBaseHandler<ApiAccessControlDeleteArguments> handler
		)
		{
            command.Id = id;
			var result = await handler.ExecuteAsync(command);
			return result;
		}

        #endregion
    }
}
