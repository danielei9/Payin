using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using PayIn.Application.Dto.Arguments.Shop;
using Xp.DistributedServices.ModelBinder;
using PayIn.Application.Dto.Results.Shop;
using System.Security.Claims;
using System.Net.Http;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System;
using System.Linq;
using PayIn.Application.Dto.Payments.Arguments.Shop;
using PayIn.Application.Dto.Payments.Results.Shop;

namespace PayIn.DistributedServices.Controllers.Api
{
    [HideSwagger]
    [RoutePrefix("Api/Shop")]
    public class ShopController : ApiController
    {       
        #region GET /
        [HttpGet]
        [Route]
        public async Task<ResultBase<ShopGetAllConcessionsResult>> GetConcessions(
            [FromUri] ShopGetAllConcessionsArguments arguments,
            [Injection] IQueryBaseHandler<ShopGetAllConcessionsArguments, ShopGetAllConcessionsResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion GET/

		#region GET /RetrieveMyCardsSelector/{filter?}
		[HttpGet]
		[Route("RetrieveMyCardsSelector/{filter?}")]
		public async Task<ResultBase<ShopGetMyCardsSelectorResult>> RetrieveMyCardsSelector(
			string filter,
			[FromUri] ShopGetMyCardsSelectorArguments arguments,
			[Injection] IQueryBaseHandler<ShopGetMyCardsSelectorArguments, ShopGetMyCardsSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /RetrieveSelector/{filter?}

		#region GET /RetrieveSelector/{filter?}
		[HttpGet]
        [Route("RetrieveSelector/{filter?}")]
        public async Task<ResultBase<ShopGetSelectorResult>> RetrieveSelector(
            string filter,
            [FromUri] ShopGetSelectorArguments arguments,
            [Injection] IQueryBaseHandler<ShopGetSelectorArguments, ShopGetSelectorResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /RetrieveSelector/{filter?}

        #region GET /All/{filter?}
        [HttpGet]
        [Route("All/{filter?}")]
        public async Task<ResultBase<ShopGetSelectorResult>> GetEvents(
            string filter,
            [FromUri] ShopGetSelectorArguments arguments,
            [Injection] IQueryBaseHandler<ShopGetSelectorArguments, ShopGetSelectorResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /All/{filter?}

        #region GET /Concession
        [HttpGet]
        [Route("Concession/{id:int}")]

        public async Task<ResultBase<ShopConcessionGetEventsResult>> GetEventsConcession(
            [FromUri] ShopConcessionGetEventsArguments arguments,
            [Injection] IQueryBaseHandler<ShopConcessionGetEventsArguments, ShopConcessionGetEventsResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion GET/Concession

		#region GET /ByConcession
		[HttpGet]
		[Route("ByConcession/{id:int}")]

		public async Task<ResultBase<ShopByConcessionResult>> GetByConcession(
			int id,
			[FromUri] ShopByConcessionArguments arguments,
			[Injection] IQueryBaseHandler<ShopByConcessionArguments, ShopByConcessionResult> handler
		)
		{
			arguments.PaymentConcessionId = id;

			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET/ByConcession

		#region GET /Event
		[HttpGet]
        [Route("Event/{id:int}")]

        public async Task<ResultBase<ShopEventGetEntranceTypesResult>> GetEntranceTypesEvent(
            [FromUri] ShopEventGetEntranceTypesArguments arguments,
            [Injection] IQueryBaseHandler<ShopEventGetEntranceTypesArguments, ShopEventGetEntranceTypesResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET/Event

        #region GET /Entrance
        [HttpGet]
        [Route("Entrance/{id:int}")]

        public async Task<ResultBase<ShopGetEntranceResult>> GetEntrance(
            [FromUri] ShopGetEntranceArguments arguments,
            [Injection] IQueryBaseHandler<ShopGetEntranceArguments, ShopGetEntranceResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET/Entrance

        #region GET /{ids}
        [HttpGet]
        [Route("ControlForm/{ids}")]
        public async Task<ResultBase<ApiControlFormGetResult>> Get(
            string ids,
            [FromUri] ApiControlFormGetArguments arguments,
            [Injection] IQueryBaseHandler<ApiControlFormGetArguments, ApiControlFormGetResult> handler
        )
        {
            arguments.Ids = ids
                .SplitString(",")
                .Select(x => Convert.ToInt32(x));

            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /{id}
    }
}
