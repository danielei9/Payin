using System.Collections.Generic;
using Xp.Application.Results;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileEventGetResult_EntranceTypes
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string PhotoUrl { get; set; }
		public decimal Price { get; set; }
		public decimal ExtraPrice { get; set; }
		public XpDateTime CheckInStart { get; set; }
		public XpDateTime CheckInEnd { get; set; }
		public XpDateTime SellEnd { get; set; }
		public int? MaxEntrance { get; set; }
		public int SelledEntrance { get; set; }

        public IEnumerable<IdResult> Forms { get; set; }
	}
}
