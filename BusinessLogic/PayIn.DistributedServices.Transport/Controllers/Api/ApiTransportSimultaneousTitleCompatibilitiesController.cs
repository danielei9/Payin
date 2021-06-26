using PayIn.Application.Dto.Transport.Arguments.TransportSimultaneousTitleCompatibilities;
using PayIn.Application.Dto.Transport.Results.TransportSimultaneousTitleCompatibilities;
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
	[RoutePrefix("Api/TransportSimultaneousTitleCompatibilities")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.TransportOperator
	)]
	public class ApiTransportSimultaneousTitleCompatibilitiesController : ApiController
	{
        #region GET /{titleId}
        [HttpGet]
        [Route("{titleId}")]
        public async Task<ResultBase<TransportSimultaneousTitleCompatibilitiesGetByTitleResult>> Get(
            [FromUri] TransportSimultaneousTitleCompatibilitiesArguments arguments,
            [Injection] IQueryBaseHandler<TransportSimultaneousTitleCompatibilitiesArguments, TransportSimultaneousTitleCompatibilitiesGetByTitleResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /{titleId}

        #region GET /
        [HttpGet]
        [Route("")]
        public async Task<ResultBase<TransportSimultaneousTitleCompatibilitiesGetByTitleResult>> GetAll(
            //string TitleId,
            [FromUri] TransportSimultaneousTitleCompatibilitiesArguments arguments,
            [Injection] IQueryBaseHandler<TransportSimultaneousTitleCompatibilitiesArguments, TransportSimultaneousTitleCompatibilitiesGetByTitleResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion GET /

		#region GET update/{id:int}
		[HttpGet]
		[Route("update/{id:int}")]
		public async Task<ResultBase<TransportSimultaneousTitleCompatibilitiesUpdateIdResult>> GetUpdate(
			[FromUri] int id,
			[FromUri] TransportSimultaneousTitleCompatibilitiesUpdateIdArguments arguments,
			[Injection] IQueryBaseHandler<TransportSimultaneousTitleCompatibilitiesUpdateIdArguments, TransportSimultaneousTitleCompatibilitiesUpdateIdResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET update/{id:int}

		#region GET /CreateGetName/{id:int}
		[HttpGet]
        [Route("CreateGetName/{id:int}")]
        public async Task<ResultBase<TransportSimultaneousTitleCompatibilitiesCreateGetNameResult>> CreateGetName(
            [FromUri] int id,
            [FromUri] TransportSimultaneousTitleCompatibilitiesCreateGetNameArguments arguments,
            [Injection] IQueryBaseHandler<TransportSimultaneousTitleCompatibilitiesCreateGetNameArguments, TransportSimultaneousTitleCompatibilitiesCreateGetNameResult> handler
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
            [FromUri] TransportSimultaneousTitleCompatibilitiesDeleteArguments arguments,
            [Injection] IServiceBaseHandler<TransportSimultaneousTitleCompatibilitiesDeleteArguments> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion DELETE /

        #region POST /
        [HttpPost]
        [Route]
        public async Task<dynamic> Post(TransportSimultaneousTitleCompatibilitiesCreateArguments arguments,
        [Injection] IServiceBaseHandler<TransportSimultaneousTitleCompatibilitiesCreateArguments> handler
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
            TransportSimultaneousTitleCompatibilitiesUpdateArguments arguments,
            [Injection] IServiceBaseHandler<TransportSimultaneousTitleCompatibilitiesUpdateArguments> handler
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
		public async Task<ResultBase<TransportSimultaneousTitleCompatibilitiesGetSelectorResult>> Selector(
			string filter,
			[FromUri] TransportSimultaneousTitleCompatibilitiesGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<TransportSimultaneousTitleCompatibilitiesGetSelectorArguments, TransportSimultaneousTitleCompatibilitiesGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Selector/{filter}

	}
}
