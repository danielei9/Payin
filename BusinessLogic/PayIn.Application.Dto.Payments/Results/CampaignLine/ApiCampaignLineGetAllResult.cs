using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.CampaignLine
{
	public class ApiCampaignLineGetAllResult
	{
		public int Id { get; set; }
		public CampaignLineType Type { get; set; }
		public string TypeAlias { get; set; }
		public decimal Quantity { get; set; }
		public int CampaignId { get; set; }
		public XpTime SinceTime { get; set; }
		public XpTime UntilTime { get; set; }
		public bool AllProduct { get; set; }
		public bool AllEntranceType { get; set; }
		public int ServiceUserCount { get; set; }
		public int ServiceGroupCount { get; set; }
		public int ProductCount { get; set; }
		public int ProductFamilyCount { get; set; }
		public int EntranceTypeCount { get; set; }
	}
}
