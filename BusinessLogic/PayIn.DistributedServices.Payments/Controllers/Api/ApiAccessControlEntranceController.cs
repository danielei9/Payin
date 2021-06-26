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
	[RoutePrefix("Api/AccessControl/Entrance")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
	)]
	public class ApiAccessControlEntranceController : ApiController
	{
		#region GET /

		[HttpGet]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.Superadministrator
		)]
		public async Task<ResultBase<ApiAccessControlEntranceGetAllResult>> GetAll(
			[FromUri] ApiAccessControlEntranceGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ApiAccessControlEntranceGetAllArguments, ApiAccessControlEntranceGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}

        #endregion

        #region GET /{id:int}

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ResultBase<ApiAccessControlEntranceGetResult>> Get(
            int id,
			[FromUri] ApiAccessControlEntranceGetArguments arguments,
			[Injection] IQueryBaseHandler<ApiAccessControlEntranceGetArguments, ApiAccessControlEntranceGetResult> handler)
		{
            arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}

		#endregion

		#region POST /

		[HttpPost]
		[Route]
		public async Task<dynamic> Post(
			ApiAccessControlEntranceCreateArguments command,
			[Injection] IServiceBaseHandler<ApiAccessControlEntranceCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}

		#endregion

		#region PUT /{id:int}

		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment)]
		public async Task<dynamic> Put(
			int id,
			ApiAccessControlEntranceUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ApiAccessControlEntranceUpdateArguments> handler)
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
			[FromUri] ApiAccessControlEntranceDeleteArguments command,
			[Injection] IServiceBaseHandler<ApiAccessControlEntranceDeleteArguments> handler
		)
		{
            command.Id = id;
			var result = await handler.ExecuteAsync(command);
			return result;
		}

        #endregion
    }
}
