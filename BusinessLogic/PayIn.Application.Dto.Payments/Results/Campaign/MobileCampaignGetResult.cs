using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class MobileCampaignGetResult
	{
		public int Id { set; get; }
		public string Title { set; get; }
		public string Description { set; get; }
		public XpDate Since { set; get; }
		public XpDate Until { set; get; }
		public int? NumberOfTimes { set; get; }
		public bool Started { set; get; }
		public bool Finished { get; set; }
		public string PhotoUrl { get; set; }
		public List<MobileCampaignGetResult_CampaignLine> CampaignLines { get; set; }
	}
}
