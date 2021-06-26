using PayIn.Common;

namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation
{
	public class TransportOperationPersonalizeArguments_Promotion
	{
		public int Id { get; set; }
		public PromoActionType Type { get; set; }
		public int Quantity { get; set; }
		public PromoLauncherType Launcher { get; set; }
	}
}
