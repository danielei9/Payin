using PayIn.Application.Dto.Bus.Arguments;
using PayIn.Application.Dto.Bus.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Bus.Controllers.Mobile
{
	[HideSwagger]
	[RoutePrefix("Bus/Mobile/Stop")]
	[XpAuthorize(
		ClientIds = AccountClientId.BusApp,
		Roles = AccountRoles.PaymentWorker + "," + AccountRoles.Commerce
	)]
	public class BusMobileStopController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		public async Task<dynamic> GetAll(
			[FromUri] BusMobileStopGetAllArguments arguments,
			[Injection] IQueryBaseHandler<BusMobileStopGetAllArguments, BusMobileStopGetAllResult> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion GET /

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

		#region PUT /Reset
		[HttpPut]
		[Route("Reset")]
		public async Task<dynamic> Reset(
			[FromBody] BusApiStopResetArguments arguments,
			[Injection] IServiceBaseHandler<BusApiStopResetArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /Reset
	}
}
