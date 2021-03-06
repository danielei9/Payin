using System.Collections.Generic;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class PaymentConcessionMobileGetAllWithPromotionsResultBase : ResultBase<PaymentConcessionMobileGetAllWithPromotionsResult>
	{
		public class Promotion
		{
			public int Id { get; set; }
			public string Title { get; set; }
			public string ConcessionName { get; set; }
			public XpDateTime Since { get; set; }
			public XpDateTime Until { get; set; }
			public string PhotoUrl { get; set; }
			public decimal? Price { get; set; }
			public int Random { get; set; }
		}

		public IEnumerable<Promotion> Promotions { get; set; }
	}
}
