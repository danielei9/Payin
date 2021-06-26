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
	[RoutePrefix("Bus/Api/Line")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.User
	)]
	public class BusApiLineController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		public async Task<dynamic> GetAll(
			[FromUri] BusApiLineGetAllArguments arguments,
			[Injection] IQueryBaseHandler<BusApiLineGetAllArguments, BusApiLineGetAllResult> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
        #endregion GET /

		#region GET /Itinerary/{id:int}
		[HttpGet]
		[Route("Itinerary/{id:int}")]
		public async Task<dynamic> GetItinerary(
			int id,
			[FromUri] BusApiVehicleGetItineraryArguments arguments,
			[Injection] IQueryBaseHandler<BusApiVehicleGetItineraryArguments, BusApiVehicleGetItineraryResult> handler
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion GET /Itinerary/{id:int}

		#region GET /Selector/{filter?}
		[HttpGet]
		[Route("Selector/{filter?}")]
		public async Task<ResultBase<SelectorResult>> Selector(
			string filter,
			[FromUri] BusApiLineGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<BusApiLineGetSelectorArguments, SelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Selector/{filter?}
	}
}
