using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xp.Domain;

namespace Xp.Infrastructure.Repositories
{
	public class TicketSignalR : Hub
	{
		#region OnConnected
		public override async Task OnConnected()
		{
			await base.OnConnected();

			var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
			var claims = identity.Claims.ToList();
			if (!claims.Any(x => x.Type == "Tenant"))
				return;

			var application = DIConfig.Resolve<IChatRepository>();
			await application.Add(Context.ConnectionId);
		}
		#endregion OnConnected

		#region OnReconnected
		public override async Task OnReconnected()
		{
			await base.OnReconnected();

			var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
			var claims = identity.Claims.ToList();
			if (!claims.Any(x => x.Type == "Tenant"))
				return;

			var application = DIConfig.Resolve<IChatRepository>();
			await application.Add(Context.ConnectionId);
		}
		#endregion OnReconnected

		#region OnDisconnected
		public override async Task OnDisconnected()
		{
			await base.OnDisconnected();

			var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
			var claims = identity.Claims.ToList();
			if (!claims.Any(x => x.Type == "Tenant"))
				return;

			var application = DIConfig.Resolve<IChatRepository>();
			await application.Remove(Context.ConnectionId);
		}
		#endregion OnDisconnected
	}
}
