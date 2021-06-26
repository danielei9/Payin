using Microsoft.AspNet.SignalR;
using PayIn.Common.DI.Public;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Application.Dto.Tsm.Arguments;

namespace PayIn.DistributedServices.Tsm.Hubs
{
	[XpSignalRAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class TsmHub : Hub
	{
		public override Task OnConnected()
		{
			return base.OnConnected();
		}
		public override Task OnDisconnected(bool stopCalled)
		{
			return base.OnDisconnected(stopCalled);
		}
		public override Task OnReconnected()
		{
			return base.OnReconnected();
		}

		public async Task<ResultBase<TsmExecuteArguments>> Execute(TsmExecuteArguments arguments)
		{
			var handler = DIConfig.Resolve<IServiceBaseHandler<TsmExecuteArguments>>();
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
	}
}
