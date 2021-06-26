using PayIn.Common;
using Xp.Domain;

namespace PayIn.Domain.Promotions
{
	public class PromoAction : Entity
	{
		public int Quantity { get; set; }
		public PromoActionType Type { get; set; }

		#region Promotion
		public int? PromotionId { get; set; }
		public Promotion Promotion { get; set; }
		#endregion Promotion
	}
}
