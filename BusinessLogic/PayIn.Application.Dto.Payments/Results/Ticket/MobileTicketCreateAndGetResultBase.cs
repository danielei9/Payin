using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileTicketCreateAndGetResultBase : ResultBase<MobileTicketCreateAndGetResult>
	{
		//public bool HasPayment { get; set; }
		public IEnumerable<MobilePaymentMediaGetAllResult> PaymentMedias { get; set; }
		public IEnumerable<MobileTicketCreateAndGetResultBase_Promotion> Promotions { get; set; }
	}
}
