using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileTicketPayV3Arguments_Promotion
	{
		public int Id { get; set; }
		public int Quantity { get; set; }
		public PromoActionType Type { get; set; }
	}
}
