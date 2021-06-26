using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobilePaymentMediaGetAllResult_Promotion
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public XpDateTime EndDate { get; set; }
		public string Concession { get; set; }
		public IEnumerable<MobilePaymentMediaGetAllResult_PromotionPrice> Prices { get; set; }
		public IEnumerable<MobilePaymentMediaGetAllResult_PromotionAction> Actions { get; set; }
	}
}
