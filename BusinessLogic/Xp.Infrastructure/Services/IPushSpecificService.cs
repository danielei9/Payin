using PayIn.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xp.Infrastructure.Services
{
	public interface IPushSpecificService
	{
		DeviceType Type { get; }

		Task<string> SendNotification(string pushId, string pushCertificate, IEnumerable<string> targetIds, NotificationType type, NotificationState state, string message, string relatedName, string relatedId, int notificationId, int sourceId, string sourceNombre);
	}
}
