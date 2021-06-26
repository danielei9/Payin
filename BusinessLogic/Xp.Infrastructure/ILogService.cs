using System;
using System.Collections.Generic;

namespace Xp.Infrastructure
{
	public interface ILogService
	{
        void TrackException(Exception exception, IDictionary<string, string> properties = null);
        void TrackEvent(string name, IDictionary<string, string> arguments);
    }
}
