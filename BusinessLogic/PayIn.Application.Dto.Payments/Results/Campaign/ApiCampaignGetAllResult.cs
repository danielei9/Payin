using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class ApiCampaignGetAllResult
	{
		public int Id { set; get; }
		public string Title { set; get; }
		public CampaignState State { get; set; }
		public string Description { set; get; }
		public XpDate Since { set; get; }
		public XpDate Until { set; get; }
		public int? NumberOfTimes { set; get; }
		public bool Active { set; get; }
		public int NumberPaymentConcessions { set; get; }
		public int NumberActivePaymentConcessions { set; get; }
		public bool IsSupplier { set; get; }
		public int CampaignLines { get; set; }
        public int events { get; set; }
    }
}
