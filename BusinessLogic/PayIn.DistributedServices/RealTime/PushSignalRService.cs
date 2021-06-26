using Microsoft.AspNet.SignalR;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Domain;
using Xp.Infrastructure.Services;

namespace PayIn.DistributedServices.RealTime
{
	public class PushSignalRService : IPushSignalRService
	{
		public class PushMessage
		{
			public string Message { get; set; }
			public string Class { get; set; }
			public string Id { get; set; }
			public string Time { get; set; }
			public int Type { get; set; }
			public int State { get; set; }
		}

		public DeviceType Type { get { return DeviceType.SignalR; } }

		private readonly IEntityRepository<Device> DeviceRepository;
		private readonly IEntityRepository<Platform> PlatformRepository;
		private readonly ISessionData SessionData;

		#region Constructors
		public PushSignalRService(
			IEntityRepository<Device> deviceRepository,
			IEntityRepository<Platform> platformRepository,
			ISessionData sessionData
		)
		{
			if (deviceRepository == null) throw new ArgumentNullException("deviceRepository");
			if (platformRepository == null) throw new ArgumentNullException("platformRepository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			DeviceRepository = deviceRepository;
			PlatformRepository = platformRepository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region SendNotification
		public async Task<string> SendNotification(string pushId, string pushCertificate, IEnumerable<string> targetIds, NotificationType type, NotificationState state, string message, string relatedName, string relatedId, int notificationId, int sourceId, string sourceNombre)
		{
			string y = await SendNotificationInternal(
				pushId,
				pushCertificate,
				targetIds,
				type,
				state,
				message,
				relatedName,
				relatedId,
				notificationId,
				sourceId,
				sourceNombre
				);
			return y;
		}
		#endregion SendNotification

		#region SendNotificationInternal
		private async Task<string> SendNotificationInternal(string pushId, string pushCertificate, IEnumerable<string> targetIds, NotificationType type, NotificationState state, string message, string relatedName, string relatedId, int notificationId, int sourceId, string sourceNombre)
		{
			// No se puede usar directamente el hub
			// http://www.asp.net/signalr/overview/signalr-20/hubs-api/hubs-api-guide-server#callfromoutsidehub

			return await Task<string>.Run(() =>
			{
				try
				{
					var context = GlobalHost.ConnectionManager.GetHubContext<PushSignalRHub>() as IHubContext;
					var clientes = context.Clients.Clients(targetIds.ToList());
					clientes.SendMessage(new PushMessage
					{
						Message = message,
						Class = relatedName,
						Id = relatedId,
						Time = DateTime.UtcNow.ToString(),
						Type = (int)type, 
						State = (int)state
					});

					return "OK";
				}
				catch
				{
					throw;
				}
			});
		}
		#endregion SendNotificationInternal
	}
}
