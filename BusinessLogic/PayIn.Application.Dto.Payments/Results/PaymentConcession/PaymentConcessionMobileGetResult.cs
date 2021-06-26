using System.Collections.Generic;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PaymentConcessionMobileGetResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string PhotoUrl { get; set; }

		public IEnumerable<PaymentConcessionMobileGetResult_Product> Products { get; set; }
		public IEnumerable<PaymentConcessionMobileGetResult_Event> Events { get; set; }
		public IEnumerable<PaymentConcessionMobileGetResult_Promotion> Promotions { get; set; }
	}
}
