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
	[RoutePrefix("Bus/Mobile/Request")]
	public class BusMobileRequestController : ApiController
	{
		#region GET /ByUser/{lineId:int}
		[HttpGet]
		[Route("ByUser/{lineId:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> ByUser(
			int lineId,
			[FromUri] BusMobileRequestGetByUserArguments arguments,
			[Injection] IQueryBaseHandler<BusMobileRequestGetByUserArguments, BusMobileRequestGetByUserResult> handler
		)
		{
			arguments.LineId = lineId;

			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion GET /ByUser/{lineId:int}

		#region POST /{lineId:int}
		[HttpPost]
		[Route("{lineId:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.BusApp + "," + AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.PaymentWorker + "," + AccountRoles.Commerce + "," + AccountRoles.User
		)]
		public async Task<dynamic> Post(
			[FromUri] int lineId,
			[FromBody] BusMobileRequestCreateArguments arguments,
			[Injection] IServiceBaseHandler<BusMobileRequestCreateArguments> handler
		)
		{
			arguments.LineId = lineId;

			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
        #endregion POST /{lineId:int}

        #region DELETE /{lineId:int}
        [HttpDelete]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.BusApp + "," + AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.PaymentWorker + "," + AccountRoles.Commerce + "," + AccountRoles.User
		)]
		public async Task<dynamic> Delete(
			int id,
			[FromUri] BusApiRequestDeleteArguments arguments,
			[Injection] IServiceBaseHandler<BusApiRequestDeleteArguments> handler
		)
		{
			arguments.Id = id;

			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion DELETE /{id:int}
	}
}
