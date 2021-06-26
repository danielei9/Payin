using System;

namespace PayIn.Application.Dto.Bus.Results
{
	public class BusApiStopGetResult
    {
		public int Id { get; set; }
		public string Code { get; set; }
		public string MasterCode { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
		public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? GeofenceRadious { get; set; }
		public TimeSpan WaitingTime { get; set; }
		public int WaitingTime_Seconds { get; set; }
	}
}
