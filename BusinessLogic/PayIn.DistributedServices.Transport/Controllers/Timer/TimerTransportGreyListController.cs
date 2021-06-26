using PayIn.Application.Dto.Transport.Arguments.TransportList;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Transport.Controllers.Timer
{
    [HideSwagger]
	[RoutePrefix("Timer/TransportGreyList")]
	public class TimerTransportGreyListController : ApiController
	{
		#region POST /Download
		[HttpPost]
		[Route("Download")]
		public async Task<dynamic> Download(
			TransportGreyListDownloadArguments arguments,
			[Injection] IServiceBaseHandler<TransportGreyListDownloadArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /Download
	}
}
