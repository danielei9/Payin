using PayIn.Application.Dto.Payments.Arguments.Liquidation;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers.Timer
{
	[HideSwagger]
	[RoutePrefix("Timer/Liquidation")]
	public class TimerLiquidationController : ApiController
	{
		#region POST /
		[HttpPost]
		[Route]
		public async Task<dynamic> Create(
			TimerLiquidationCreateArguments arguments,
			[Injection] IServiceBaseHandler<TimerLiquidationCreateArguments> handler
		)
		{
			//await handler.ExecuteAsync(arguments);
			return null;
		}
		#endregion POST /
	}
}
