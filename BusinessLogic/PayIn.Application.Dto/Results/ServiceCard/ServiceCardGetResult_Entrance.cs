using System;
using PayIn.Common;

namespace PayIn.Application.Dto.Results
{
	public class ServiceCardGetResult_Entrance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public long Code { get; set; }
        public string EntranceTypeName { get; set; }
        public string EventName { get; set; }
        public decimal? Amount { get; set; }
		public DateTime? EventStart { get; set; }
		public DateTime? Timestamp { get; set; }
		public bool Finished { get; set; }
		public EntranceState State { get; set; }
	}
}
