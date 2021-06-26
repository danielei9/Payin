using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
	public class MobileMainSynchronizeResult_EntranceType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string PhotoUrl { get; set; }
		//public int? Capacity { get; set; }
		public decimal Price { get; set; }
        public decimal ExtraPrice { get; set; }
        public DateTime? StartDay { get; set; }
        public DateTime CheckInStart { get; set; }
		public string Code { get; set; }
		public IEnumerable<MobileMainSynchronizeResult_ValidationEntrance> Entrances { get; set; }
		//public IEnumerable<MainMobileSynchronizeResult_ServiceGroupId> Groups { get; set; }
	}
}
