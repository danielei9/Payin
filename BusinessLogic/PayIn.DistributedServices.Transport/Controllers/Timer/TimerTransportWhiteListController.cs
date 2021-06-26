using PayIn.Application.Dto.Transport.Arguments.TransportList;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Transport.Controllers.Timer
{
    [HideSwagger]
	[RoutePrefix("Timer/TransportWhiteList")]
	public class TimerTransportWhiteListController : ApiController
	{
		#region POST /Download
		[HttpPost]
		[Route("Download")]
		public async Task<dynamic> Download(
			TransportWhiteListDownloadArguments arguments,
			[Injection] IServiceBaseHandler<TransportWhiteListDownloadArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /Download
	}
}
