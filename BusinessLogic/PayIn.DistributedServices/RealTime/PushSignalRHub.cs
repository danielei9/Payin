using Microsoft.AspNet.SignalR;
using PayIn.Application.Dto.Arguments.Device;
using PayIn.Common;
using PayIn.Common.DI.Public;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.DistributedServices.RealTime
{
	[XpSignalRAuthorize(
		Roles = AccountRoles.User,
		ClientIds = AccountClientId.Tpv
	)]
	public class PushSignalRHub : Hub
	{
		#region OnConnected
		public override async Task OnConnected()
		{
			await base.OnConnected();

			var application = DIConfig.Resolve<IServiceBaseHandler<DeviceCreateArguments>>();
			await application.ExecuteAsync(new DeviceCreateArguments(
				Context.ConnectionId,
				DeviceType.SignalR
			));
		}
		#endregion OnConnected

		#region OnReconnected
		public override async Task OnReconnected()
		{
			await base.OnReconnected();

			var application = DIConfig.Resolve<IServiceBaseHandler<DeviceCreateArguments>>();
			await application.ExecuteAsync(new DeviceCreateArguments(
				Context.ConnectionId,
				DeviceType.SignalR
			));
		}
		#endregion OnReconnected

		//#region OnDisconnected
		//public override async Task OnDisconnected(bool stopCalled)
		//{
		//	await base.OnDisconnected(stopCalled);

		//	var application = DIConfig.Resolve<ServiceNotificationRepository>();
		//	await application.Remove(Context.ConnectionId);
		//}
		//#endregion OnDisconnected
	}
}
