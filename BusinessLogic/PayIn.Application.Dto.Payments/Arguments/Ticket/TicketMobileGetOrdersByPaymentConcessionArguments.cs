using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketMobileGetOrdersByPaymentConcessionArguments : IArgumentsBase
	{
		public int PaymentConcessionId { get; set; }
	}
}
