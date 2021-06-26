using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileCampaignGetAllResult
	{
		public int Id { set; get; }
		public string Title { set; get; }
		public string Description { set; get; }
		public XpDate Since { set; get; }
		public XpDate Until { set; get; }
		public bool State { set; get; }
		public string PhotoUrl { get; set; }
	}
}
