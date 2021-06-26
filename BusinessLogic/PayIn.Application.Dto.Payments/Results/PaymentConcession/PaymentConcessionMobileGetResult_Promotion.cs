using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PaymentConcessionMobileGetResult_Promotion
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public XpDateTime Since { get; set; }
		public XpDateTime Until { get; set; }
		public string PhotoUrl { get; set; }
		public decimal? Price { get; set; }
	}
}
