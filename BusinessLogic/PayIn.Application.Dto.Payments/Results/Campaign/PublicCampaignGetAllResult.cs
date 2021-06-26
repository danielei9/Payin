using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PublicCampaignGetAllResult
	{
		public int Id { set; get; }
        public string Title { set; get; }
		public XpTime Since { set; get; }
		public XpTime Until { set; get; }
		public XpDateTime Caducity { get; set; }
	}
}
