using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
	public class ServiceCardGetResult_PurseValue
	{
		public class PendingRecharge
		{
			//public long Uid { get; set; }
			//public int PurseSlot { get; set; }
			public DateTime DateTime { get; set; }
			public decimal Amount { get; set; }
		}
		public int? Slot { get; set; }
		public string Name { get; set; }
		public decimal Amount { get; set; }
		public IEnumerable<PendingRecharge> PendingRecharges {get;set;}
	}
}
