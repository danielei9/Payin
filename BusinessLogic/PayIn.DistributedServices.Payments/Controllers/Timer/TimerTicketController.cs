using PayIn.Application.Dto.Payments.Arguments;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers.Timer
{
	[HideSwagger]
	[RoutePrefix("Timer/Ticket")]
	public class TimerTicketController : ApiController
	{
		#region PUT /
		[HttpPut]
		[Route]
		public async Task<dynamic> Activate(
			TicketActivateArguments command,
			[Injection] IServiceBaseHandler<TicketActivateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return item;
		}
		#endregion PUT /
	}
}
