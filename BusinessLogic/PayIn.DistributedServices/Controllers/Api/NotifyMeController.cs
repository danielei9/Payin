using PayIn.Application.Dto.Arguments;
using PayIn.Common;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/NotifyMe")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative
	)]
	public class NotifyMeController : ApiController
	{
		#region POST /Event
		[HttpPost]
		[Route("Event")]
		public async Task<dynamic> PostEvent(
			NotifyMeCreateArguments command,
			[Injection] IServiceBaseHandler<NotifyMeCreateArguments> handler
		)
		{
			command.Type = NotifyMeType.Event;
			var item = await handler.ExecuteAsync(command);
			return new { Ok = item };
		}
		#endregion POST /Event	

		#region POST /Activity
		[HttpPost]
		[Route("Activity")]
		public async Task<dynamic> PostActivity(
			NotifyMeCreateArguments command,
			[Injection] IServiceBaseHandler<NotifyMeCreateArguments> handler
		)
		{
			command.Type = NotifyMeType.Activity;
			var item = await handler.ExecuteAsync(command);
			return new { Ok = item };
		}
		#endregion POST /Activity	

		#region DELETE /Event/{id:int}
		[HttpDelete]
		[Route("Event/{id:int}")]
		public async Task<dynamic> DeleteEvent(
			[FromUri] NotifyMeDeleteArguments command,
			[Injection] IServiceBaseHandler<NotifyMeDeleteArguments> handler
		)
		{
			command.Type = NotifyMeType.Event;
			var item = await handler.ExecuteAsync(command);
			return new { Ok = item };
		}
		#endregion DELETE /Event/{id:int}

		#region DELETE /Activity
		[HttpDelete]
		[Route("Activity/{id:int}")]
		public async Task<dynamic> DeleteActivity(
			[FromUri] NotifyMeDeleteArguments command,
			[Injection] IServiceBaseHandler<NotifyMeDeleteArguments> handler
		)
		{
			command.Type = NotifyMeType.Activity;
			var item = await handler.ExecuteAsync(command);
			return new { Ok = item };
		}
		#endregion DELETE /Activity	

	}
}
