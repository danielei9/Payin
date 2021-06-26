using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xp.Infrastructure
{
	public interface IAnalyticsService
	{
		Task TrackEventAsync(string name, Dictionary<string, object> parameters = null);
		Task TrackUserAsync(string login, string name, string email);
	}
}
