using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;

namespace Xp.Infrastructure.Services
{
	public class ApplicationInsightsLogService : ILogService
    {
        #region TrackException
        public void TrackException(Exception exception, IDictionary<string, string> properties = null)
        {
            var telemetry = new TelemetryClient();
            telemetry.TrackException(exception);
        }
        #endregion TrackException

        #region TrackEvent
        public void TrackEvent(string name, IDictionary<string, string> properties = null)
        {
            var telemetry = new TelemetryClient();
            telemetry.TrackEvent(name, properties);
        }
        #endregion TrackEvent
    }
}
