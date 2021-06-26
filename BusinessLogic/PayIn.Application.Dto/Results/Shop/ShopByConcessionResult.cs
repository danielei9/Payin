using System;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Results.Shop
{
	public class ShopByConcessionResult
	{
		// Entrance
		public IEnumerable<ShopEventGetEntranceTypesResult> EntranceTypes { get; set; }

		// Event
		public int EventId { get; set; }
		public string EventName { get; set; }
		public string EventDescription { get; set; }
		public string EventShortDescription { get; set; }
		public string EventConditions { get; set; }
        public bool ShowConditions { get; set; }
        public XpDateTime EventDate { get; set; }
		public string EventPlace { get; set; }
		public string EventPhotoUrl { get; set; }
	}
}