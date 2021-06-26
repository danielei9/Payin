using PayIn.Application.Dto.Bus.Arguments;
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
	[RoutePrefix("Bus/Api/Request")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.User
	)]
	public class BusApiRequestController : ApiController
	{
		#region POST /{lineId:int}
		[HttpPost]
		[Route("{lineId:int}")]
		public async Task<dynamic> Post(
			[FromUri] int lineId,
			[FromBody] BusApiRequestCreateArguments arguments,
			[Injection] IServiceBaseHandler<BusApiRequestCreateArguments> handler
		)
		{
			arguments.LineId = lineId;

			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /{lineId:int}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
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
