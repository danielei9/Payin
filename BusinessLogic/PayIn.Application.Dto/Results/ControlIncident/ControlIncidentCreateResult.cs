using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ControlIncident
{
    public class ControlIncidentCreateResult
	{
		public class Track
		{
			public int Id { get; set; }
		}

		public IEnumerable<Track> Entrances { get; set; }
		public IEnumerable<Track> Exits { get; set; }
	}
}
