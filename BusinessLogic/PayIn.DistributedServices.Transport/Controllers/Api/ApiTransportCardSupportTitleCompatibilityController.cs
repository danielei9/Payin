using PayIn.Application.Dto.Transport.Arguments.TransportCardSupportTitleCompatibility;
using PayIn.Application.Dto.Transport.Results.TransportCardSupportTitleCompatibility;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Transport.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/TransportCardSupportTitleCompatibility")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.TransportOperator
	)]
	public class ApiTransportCardSupportTitleCompatibilityController : ApiController
	{
        #region GET /{titleId}
        [HttpGet]
        [Route("{titleId}")]
        public async Task<ResultBase<TransportCardSupportTitleCompatibilityGetByTitleResult>> Get(
            [FromUri] TransportCardSupportTitleCompatibilityArguments arguments,
            [Injection] IQueryBaseHandler<TransportCardSupportTitleCompatibilityArguments, TransportCardSupportTitleCompatibilityGetByTitleResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /{titleId}

        #region GET /
        [HttpGet]
        [Route("")]
        public async Task<ResultBase<TransportCardSupportTitleCompatibilityGetByTitleResult>> GetAll(
            //string TitleId,
            [FromUri] TransportCardSupportTitleCompatibilityArguments arguments,
            [Injection] IQueryBaseHandler<TransportCardSupportTitleCompatibilityArguments, TransportCardSupportTitleCompatibilityGetByTitleResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion GET /

		#region GET /update/{id:int}
		[HttpGet]
		[Route("update/{id:int}")]
		public async Task<ResultBase<TransportCardSupportTitleCompatibilityUpdateIdResult>> GetUpdate(
			//string TitleId,
			[FromUri] int id,
			[FromUri] TransportCardSupportTitleCompatibilityUpdateIdArguments arguments,
			[Injection] IQueryBaseHandler<TransportCardSupportTitleCompatibilityUpdateIdArguments, TransportCardSupportTitleCompatibilityUpdateIdResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /update/{id:int}

		#region GET /CreateGetName/{id:int}
		[HttpGet]
        [Route("CreateGetName/{id:int}")]
        public async Task<ResultBase<TransportCardSupportTitleCompatibilityCreateGetNameResult>> CreateGetName(
            //string TitleId,
            [FromUri] int id,
            [FromUri] TransportCardSupportTitleCompatibilityCreateGetNameArguments arguments,
            [Injection] IQueryBaseHandler<TransportCardSupportTitleCompatibilityCreateGetNameArguments, TransportCardSupportTitleCompatibilityCreateGetNameResult> handler
        )
        {
            arguments.Id = id;
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion GET /CreateGetName/{id:int}

		#region DELETE /
		[HttpDelete]
        [Route("{id:int}")]
        public async Task<dynamic> Delete(
            [FromUri] TransportCardSupportTitleCompatibilityDeleteArguments arguments,
            [Injection] IServiceBaseHandler<TransportCardSupportTitleCompatibilityDeleteArguments> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion DELETE /

        #region POST /
        [HttpPost]
        [Route]
        public async Task<dynamic> Post(TransportCardSupportTitleCompatibilityCreateArguments arguments,
        [Injection] IServiceBaseHandler<TransportCardSupportTitleCompatibilityCreateArguments> handler
        )
        {
            var item = await handler.ExecuteAsync(arguments);
            return item;
        }
		#endregion POST /

		#region PUT /{id:int}
		[HttpPut]
        [Route("{id:int}")]
        public async Task<dynamic> Put(
            [FromUri] int id,
            TransportCardSupportTitleCompatibilityUpdateArguments arguments,
            [Injection] IServiceBaseHandler<TransportCardSupportTitleCompatibilityUpdateArguments> handler
        )
        {
            arguments.Id = id;
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
		#endregion PUT /{id:int}

		#region GET /Selector/{filter}
		[HttpGet]
		[Route("Selector/{filter}")]
		public async Task<ResultBase<TransportCardSupportTitleCompatibilityGetSelectorResult>> Selector(
			string filter,
			[FromUri] TransportCardSupportTitleCompatibilityGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<TransportCardSupportTitleCompatibilityGetSelectorArguments, TransportCardSupportTitleCompatibilityGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Selector/{filter}

	}
}
