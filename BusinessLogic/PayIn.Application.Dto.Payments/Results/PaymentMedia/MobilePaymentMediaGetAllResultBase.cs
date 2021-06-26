using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobilePaymentMediaGetAllResultBase : ResultBase<MobilePaymentMediaGetAllResult>
	{
		public bool                                                    UserHasPayment { get; set; }
		public IEnumerable<MobilePaymentMediaGetAllResult_Promotion>   Promotions { get; set; }
		public IEnumerable<MobilePaymentMediaGetAllResult_Purse>       Purses { get; set; }
		public IEnumerable<MobileEntranceGetAllResult>    Entrances { get; set; }
		public IEnumerable<MobilePaymentMediaGetAllResult_ServiceCard> ServiceCards { get; set; }
	}
}