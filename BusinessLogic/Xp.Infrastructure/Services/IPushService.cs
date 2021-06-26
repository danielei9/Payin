using PayIn.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xp.Infrastructure.Repositories
{
	public interface IPushService
	{
      Task<string> SendNotification(IEnumerable<string> targetIds, NotificationType type, NotificationState state, string message, string relatedName, string relatedId, int notificationId);
	}
}
