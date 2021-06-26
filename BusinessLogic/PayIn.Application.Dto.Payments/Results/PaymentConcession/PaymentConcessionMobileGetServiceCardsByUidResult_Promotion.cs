using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PaymentConcessionMobileGetServiceCardsByUidResult_Promotion
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public XpDateTime Since { get; set; }
		public XpDateTime Until { get; set; }
		public string Image { get; set; }
	}
}
