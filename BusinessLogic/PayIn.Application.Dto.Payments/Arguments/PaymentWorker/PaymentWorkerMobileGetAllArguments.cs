using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentWorkerMobileGetAllArguments : IArgumentsBase
	{
		public int Skip { get; set; }
		public int Top { get; set; }
    }
}
