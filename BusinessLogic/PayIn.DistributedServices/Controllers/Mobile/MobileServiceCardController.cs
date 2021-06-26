using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Mobile
{
    [HideSwagger]
	[RoutePrefix("Mobile/ServiceCard")]
	public class MobileServiceCardController : ApiController
	{
		#region GET /v1
		[HttpGet]
		[Route("v1")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.User
		)]
		public async Task<ResultBase<MobileServiceCardGetAllResult>> GetAll(
			[FromUri] MobileServiceCardGetAllArguments arguments,
			[Injection] IQueryBaseHandler<MobileServiceCardGetAllArguments, MobileServiceCardGetAllResult> handler
		)
		{
			var items = await handler.ExecuteAsync(arguments);
			return items;
		}
		#endregion GET /v1

		#region GET /v1/{id}
		[HttpGet]
		[Route("v1/{id}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.User
		)]
		public async Task<ResultBase<ServiceCardReadInfoResult>> Get(
			int id,
			[FromUri] MobileServiceCardGetArguments arguments,
			[Injection] IQueryBaseHandler<MobileServiceCardGetArguments, ServiceCardReadInfoResult> handler
		)
		{
			arguments.Id = id;

			var items = await handler.ExecuteAsync(arguments);
			return items;
		}
		#endregion GET /v1/{id}

        #region PUT /v1/link
        [HttpPut]
        [Route("v1/link")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
            Roles = AccountRoles.User
        )]
        public async Task<dynamic> Link(
			MobileServiceCardLinkArguments arguments,
            [Injection] IServiceBaseHandler<MobileServiceCardLinkArguments> handler
        )
        {
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion PUT /v1/link

        #region PUT /v1/unlink/{id:int}
        [HttpPut]
        [Route("v1/unlink/{id:int}")]
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
        #endregion PUT /v1/unlink/{id:int}
    }
}
