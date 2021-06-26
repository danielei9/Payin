using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlPresence
{
	public class ControlPresenceMobileCheckResult
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
