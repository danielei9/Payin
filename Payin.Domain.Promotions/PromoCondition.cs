using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Promotions
{
	public class PromoCondition : Entity
	{
		public int Quantity { get; set; }
		public PromoConditionType Type { get; set; }

		#region Promotion
		public int PromotionId { get; set; }
		public Promotion Promotion { get; set; }
		#endregion Promotion


	}
}
