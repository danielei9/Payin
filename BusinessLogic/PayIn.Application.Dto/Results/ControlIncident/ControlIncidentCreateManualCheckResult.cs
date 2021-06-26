using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlIncident
{
	public class ControlIncidentCreateManualCheckResult
	{
		public class Track
		{
			public int Id { get; set; }
		}

		public IEnumerable<Track> Entrances      { get; set; }
		public IEnumerable<Track> Exits          { get; set; }
		public XpTime             TrackFrecuency { get; set; }
	}
}
