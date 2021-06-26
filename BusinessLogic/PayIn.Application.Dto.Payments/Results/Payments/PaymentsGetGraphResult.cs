using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PaymentsGetGraphResult
	{
		public decimal PayedAmount { get; set; }
		public decimal ReturnedAmount { get; set; }
		public XpDate Day { get; set; }
	}
}
