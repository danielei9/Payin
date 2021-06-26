using PayIn.Common;
using System;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class MobileCampaignGetResult_CampaignLine
	{
		public CampaignLineType Type { get; set; }
		public decimal Min { get; set; }
		public decimal Max { get; set; }
		public decimal Quantity { get; set; }
		public DateTime? SinceTime { get; set; }
		public DateTime? UntilTime { get; set; }
		public CampaignLineState State { get; set; }
	}
}
