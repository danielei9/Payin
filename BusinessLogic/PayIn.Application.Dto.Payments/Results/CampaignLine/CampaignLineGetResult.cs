using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.CampaignLine
{
	public class CampaignLineGetResult
	{
		public int Id { get; set; }
		public CampaignLineType Type { get; set; }
		//public decimal Min { get; set; }
		//public decimal Max { get; set; }
		public decimal Quantity { get; set; }
		public int CampaignId { get; set; }
		//public int? PurseId { get; set; }
		public bool AllProduct { get; set; }
		public bool AllEntranceType { get; set; }
		public XpTime SinceTime { get; set; }
		public XpTime UntilTime { get; set; }
		public bool All { get; set; }
	}
}
