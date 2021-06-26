using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobilePaymentMediaGetByPaymentConcessionArguments : IArgumentsBase
	{
		public int PaymentConcessionId { get; set; }
	}
}
