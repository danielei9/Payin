using PayIn.Application.Dto.Payments.Arguments;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers.Sabadell
{
	[HideSwagger]
	[RoutePrefix("Sabadell")]
	public class SabadellController : ApiController
	{
		#region POST /WebCard
		[HttpPost]
		[Route("WebCard")]
		public async Task<dynamic> WebCard(
			[FromBody] SabadellPaymentMediaCreateWebCardArguments arguments,
			[Injection] IServiceBaseHandler<SabadellPaymentMediaCreateWebCardArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /WebCard
	}
}
