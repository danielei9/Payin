using System.Collections.Generic;

namespace PayIn.Application.Dto.Bus.Results
{
	public class BusApiVehicleGetItineraryResult_Route
	{
		public IEnumerable<BusApiVehicleGetItineraryResult_Stop> Stops { get; set; }
	}
}
