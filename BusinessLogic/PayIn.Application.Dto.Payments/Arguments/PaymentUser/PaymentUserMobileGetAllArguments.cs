using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.PaymentUser
{
	public class PaymentUserMobileGetAllArguments : IArgumentsBase
	{
		public int Skip { get; set; }
		public int Top { get; set; }
    }
}
