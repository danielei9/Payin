using System.Collections.Generic;

namespace PayIn.Application.Dto.Bus.Results
{
	public class BusApiVehicleGetItineraryResult
	{
		public int Id { get; set; }
		public int? LineId { get; set; }

		//public IEnumerable<BusApiVehicleGetItineraryResult_Route> Routes { get; set; }
		public IEnumerable<BusApiVehicleGetItineraryResult_Stop> Stops { get; set; }
	}
}
