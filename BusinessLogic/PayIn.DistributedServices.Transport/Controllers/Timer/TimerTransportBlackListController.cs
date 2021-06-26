using PayIn.Application.Dto.Transport.Arguments.TransportList;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Transport.Controllers.Timer
{
    [HideSwagger]
	[RoutePrefix("Timer/TransportBlackList")]
	public class TimerTransportBlackListController : ApiController
	{
		#region POST /Download
		[HttpPost]
		[Route("Download")]
		public async Task<dynamic> Download(
			TransportBlackListDownloadArguments arguments,
			[Injection] IServiceBaseHandler<TransportBlackListDownloadArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /Download
	}
}
