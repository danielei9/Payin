using PayIn.Domain.Bus.Enums;
using Xp.Common;

namespace PayIn.Application.Dto.Bus.Results
{
	public class BusApiVehicleGetItineraryResult_PendingRequest
	{
		public int Id { get; set; }
		public string Login { get; set; }
		public string UserName { get; set; }
		public XpDateTime Timestamp { get; set; }
		// From
		public string FromCode { get; set; }
		public string FromName { get; set; }
		public XpDateTime FromVisitTimeStamp { get; set; }
		public RequestNodeState FromState { get; set; }
		// To
		public string ToCode { get; set; }
		public string ToName { get; set; }
		public RequestNodeState ToState { get; set; }
		public XpDateTime ToVisitTimeStamp { get; set; }
	}
}
