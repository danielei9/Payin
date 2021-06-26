using PayIn.Application.Dto.Payments.Arguments.Purse;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers.Timer
{
	[HideSwagger]
	[RoutePrefix("Timer/Purse")]
	public class TimerPurseController : ApiController
	{
		#region Put
		[HttpPut]
		[Route]
		public async Task<dynamic> Activate(
			PurseCloseArguments command,
			[Injection] IServiceBaseHandler<PurseCloseArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return item;
		}
		#endregion Put
	}
}
