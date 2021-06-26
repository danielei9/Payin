using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
	public class MobileMainSynchronizeResult_Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public decimal? Price { get; set; }
        public int? ConcessionId { get; set; }
        public string ConcessionName { get; set; }
		
		public IEnumerable<MobileMainSynchronizeResult_ServiceGroupId> Groups { get; set; }
		public IEnumerable<MobileMainSynchronizeResult_CampaignLine> CampaignLines { get; set; }
	}
}
