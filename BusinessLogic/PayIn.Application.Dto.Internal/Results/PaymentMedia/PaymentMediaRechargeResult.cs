using System.Collections.Generic;

namespace PayIn.Application.Dto.Internal.Results
{
	public class PaymentMediaRechargeResult
    {
        public List<int> Purses { get; set; }
        public List<decimal> Quantity { get; set; }
        public List<int> ConcessionId { get; set; }
    }
}
