using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileTicketCreateAndGetResult_Promotion
	{
		public int Id { get; set; }
		public int Quantity { get; set; }
		public PromoActionType Type { get; set; }
	}
}
