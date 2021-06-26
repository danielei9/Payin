using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Bus.Results
{
	public class BusApiVehicleGetItineraryResultBase: ResultBase<BusApiVehicleGetItineraryResult>
	{
		public IEnumerable<BusApiStopGetByLineResult> Stops { get; set; }
		public IEnumerable<BusApiVehicleGetItineraryResult_PendingRequest> PendingRequests { get; set; }
	}
}
