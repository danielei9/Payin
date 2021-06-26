using PayIn.Application.Dto.Bus.Arguments;
using PayIn.Application.Dto.Bus.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Bus.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Bus/Api/Stop")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.User
	)]
	public class BusApiStopController : ApiController
	{
        #region GET /{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<dynamic> Get(
            int id,
            [FromUri] BusApiStopGetArguments arguments,
            [Injection] IQueryBaseHandler<BusApiStopGetArguments, BusApiStopGetResult> handler
        )
        {
            arguments.Id = id;
            var item = await handler.ExecuteAsync(arguments);
            return item;
        }
        #endregion GET /{id}

        #region POST /
        [HttpPost]
        [Route]
        public async Task<dynamic> Put(
            BusApiStopCreateArguments arguments,
            [Injection] IServiceBaseHandler<BusApiStopCreateArguments> handler
        )
        {
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion POST /

        #region PUT /{id}
        [HttpPut]
        [Route("{id}")]
        public async Task<dynamic> Put(
            int id,
            BusApiStopUpdateArguments arguments,
            [Injection] IServiceBaseHandler<BusApiStopUpdateArguments> handler
        )
        {
            arguments.Id = id;
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion PUT /{id}

        #region GET /Link/{id}
        [HttpGet]
		[Route("Link/{id}")]
		public async Task<dynamic> GetLink(
			int id,
			[FromUri] BusApiStopGetLinkArguments arguments,
			[Injection] IQueryBaseHandler<BusApiStopGetLinkArguments, BusApiStopGetLinkResult> handler
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion GET /Link/{id}

		#region PUT /UpdateLink
		[HttpPut]
		[Route("UpdateLink/{id}")]
		public async Task<dynamic> PutLink(
			int id,
			BusApiStopUpdateLinkArguments arguments,
			[Injection] IServiceBaseHandler<BusApiStopUpdateLinkArguments> handler
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /UpdateLink

		#region GET /ByLine
		[HttpGet]
        [Route("ByLine/{id}")]
        public async Task<dynamic> GetByLine(
            int id,
            [FromUri] BusApiStopGetByLineArguments arguments,
            [Injection] IQueryBaseHandler<BusApiStopGetByLineArguments, BusApiStopGetByLineResult> handler
        )
        {
            arguments.LineId = id;
            var item = await handler.ExecuteAsync(arguments);
            return item;
        }
		#endregion GET /ByLine

		#region GET /Selector/{filter?}
		[HttpGet]
		[Route("Selector/{filter?}")]
		public async Task<ResultBase<SelectorResult>> Selector(
			string filter,
			[FromUri] BusApiStopGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<BusApiStopGetSelectorArguments, SelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Selector/{filter?}

		#region PUT /Visit/{id:int}
		[HttpPut]
		[Route("Visit/{id:int}")]
		public async Task<dynamic> Visit(
			int id,
			[FromBody] BusApiStopVisitArguments arguments,
			[Injection] IServiceBaseHandler<BusApiStopVisitArguments> handler
		)
		{
			arguments.Id = id;

			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /Visit/{id:int}
	}
}
