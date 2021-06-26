using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
	public class MobileMainSynchronizeResult_Event
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
		public string Name { get; set; }
		public string ShortDescription { get; set; }
		public string Description { get; set; }
		public DateTime? CheckInStart { get; set; }
		public DateTime? CheckInEnd { get; set; }
		public int? Capacity { get; set; }
        public bool IsOnline { get; set; }
        
        public IEnumerable<MobileMainSynchronizeResult_ValidationEntrance> Entrances { get; set; }
	}
}
