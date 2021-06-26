using PayIn.Domain.Bus.Enums;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Bus.Results
{
	public class BusApiStopGetByLineResult
    {
		public int Id { get; set; }
		public string Code { get; set; }
		public string MasterCode { get; set; }
		public string Name { get; set; }
        public NodeType Type { get; set; }
        public string Location { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
		public System.TimeSpan WaitingTime { get; set; }

		public IEnumerable<BusApiStopGetByLineResult_Link> Links { get; set; }
    }
}
