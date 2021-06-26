using PayIn.Common;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Results
{
	public class MobileMainSynchronizeResult_CampaignLine
	{
		public int Id { get; set; }
		public decimal Max { get; set; }
		public decimal Min { get; set; }
		public decimal Quantity { get; set; }
		public CampaignLineType Type { get; set; }
		public XpTime SinceTime { get; set; }
		public XpTime UntilTime { get; set; }
		public CampaignLineState State { get; set; }
		public int CampaignId { get; set; }
		public string CampaignTitle { get; set; }
		public XpDateTime CampaignSince { get; set; }
		public XpDateTime CampaignUntil { get; set; }

		public IEnumerable<MobileMainSynchronizeResult_CampaignLine_User> Users { get; set; }
		public IEnumerable<MobileMainSynchronizeResult_CampaignLine_Group> Groups { get; set; }
	}
}
