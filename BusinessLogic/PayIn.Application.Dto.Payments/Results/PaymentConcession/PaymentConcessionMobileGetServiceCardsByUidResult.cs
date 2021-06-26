using System.Collections.Generic;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PaymentConcessionMobileGetServiceCardsByUidResult
	{
		public string OwnerUserName { get; set; }
		public string OwnerUserVatNumber { get; set; }
		public string OwnerUserPhoto { get; set; }
		public IEnumerable<PaymentConcessionMobileGetServiceCardsByUidResult_Concession> Concessions { get; set; }
		public IEnumerable<PaymentConcessionMobileGetServiceCardsByUidResult_Promotion> Promotions { get; set; }
	}
}
