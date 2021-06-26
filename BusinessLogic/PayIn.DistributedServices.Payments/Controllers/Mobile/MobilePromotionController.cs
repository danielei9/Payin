using PayIn.Application.Dto.Payments.Arguments.Promotion;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers
{
	[HideSwagger]
	[RoutePrefix("Mobile/Promotion")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User + "," + AccountRoles.Transport
	)]
	public class MobilePromotionController : ApiController
	{
		#region POST /Assign
		[HttpPost]
		[Route("Assign")]
		public async Task<dynamic> Assign(
			PromotionAsignArguments command,
			[Injection] IServiceBaseHandler<PromotionAsignArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return item;
		}
		#endregion POST /Assign

		#region Unlinkcode /{id:int}
		[HttpDelete]
		[Route("UnlinkCode/{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] PromotionUnlinkCodeArguments arguments,
			[Injection] IServiceBaseHandler<PromotionUnlinkCodeArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion Unlinkcode /{id:int}
	}
}
