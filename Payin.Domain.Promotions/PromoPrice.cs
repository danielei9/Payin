using PayIn.Domain.Transport;
using Xp.Domain;

namespace PayIn.Domain.Promotions
{
	public class PromoPrice : Entity
	{		
		#region Promotion
		public int PromotionId { get; set; }
		public Promotion Promotion { get; set; }
		#endregion Concession

		#region TransportPrice
		public int TransportPriceId { get; set; }
		public TransportPrice TransportPrice { get; set; }
		#endregion TransportPrice
	}
}
