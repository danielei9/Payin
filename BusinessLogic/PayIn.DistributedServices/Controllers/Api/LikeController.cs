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
	[RoutePrefix("Api/Like")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative
	)]
	public class LikeController : ApiController
	{
		#region POST /Event
		[HttpPost]
		[Route("Event")]
		public async Task<dynamic> PostEvent(
			LikeCreateArguments command,
			[Injection] IServiceBaseHandler<LikeCreateArguments> handler
		)
		{
			command.Type = LikeType.Event;
			var item = await handler.ExecuteAsync(command);
			return new { Ok = item };
		}
		#endregion POST /Event	

		#region POST /Exhibitor
		[HttpPost]
		[Route("Exhibitor")]
		public async Task<dynamic> PostExhibitor(
			LikeCreateArguments command,
			[Injection] IServiceBaseHandler<LikeCreateArguments> handler
		)
		{
			command.Type = LikeType.Exhibitor;
			var item = await handler.ExecuteAsync(command);
			return new { Ok = item };
		}
		#endregion POST /Exhibitor	

		#region POST /Notice
		[HttpPost]
		[Route("Notice")]
		public async Task<dynamic> PostNotice(
			LikeCreateArguments command,
			[Injection] IServiceBaseHandler<LikeCreateArguments> handler
		)
		{
			command.Type = LikeType.Notice;
			var item = await handler.ExecuteAsync(command);
			return new { Ok = item };
		}
		#endregion POST /Notice	

		#region POST /Activity
		[HttpPost]
		[Route("Activity")]
		public async Task<dynamic> PostActivity(
			LikeCreateArguments command,
			[Injection] IServiceBaseHandler<LikeCreateArguments> handler
		)
		{
			command.Type = LikeType.Activity;
			var item = await handler.ExecuteAsync(command);
			return new { Ok = item };
		}
		#endregion POST /Activity	

		#region DELETE /Event/{id:int}
		[HttpDelete]
		[Route("Event/{id:int}")]
		public async Task<dynamic> DeleteEvent(
			[FromUri] LikeDeleteArguments command,
			[Injection] IServiceBaseHandler<LikeDeleteArguments> handler
		)
		{
			command.Type = LikeType.Event;
			var item = await handler.ExecuteAsync(command);
			return new { Ok = item };
		}
		#endregion DELETE /Event/{id:int}

		#region DELETE /Exhibitor
		[HttpDelete]
		[Route("Exhibitor/{id:int}")]
		public async Task<dynamic> DeleteExhibitor(
			[FromUri] LikeDeleteArguments command,
			[Injection] IServiceBaseHandler<LikeDeleteArguments> handler
		)
		{
			command.Type = LikeType.Exhibitor;
			var item = await handler.ExecuteAsync(command);
			return new { Ok = item };
		}
		#endregion DELETE /Exhibitor	

		#region DELETE /Notice
		[HttpDelete]
		[Route("Notice/{id:int}")]
		public async Task<dynamic> DeleteNotice(
			[FromUri] LikeDeleteArguments command,
			[Injection] IServiceBaseHandler<LikeDeleteArguments> handler
		)
		{
			command.Type = LikeType.Notice;
			var item = await handler.ExecuteAsync(command);
			return new { Ok = item };
		}
		#endregion DELETE /Notice	

		#region DELETE /Activity
		[HttpDelete]
		[Route("Activity/{id:int}")]
		public async Task<dynamic> DeleteActivity(
			[FromUri] LikeDeleteArguments command,
			[Injection] IServiceBaseHandler<LikeDeleteArguments> handler
		)
		{
			command.Type = LikeType.Activity;
			var item = await handler.ExecuteAsync(command);
			return new { Ok = item };
		}
		#endregion DELETE /Activity	

	}
}
