using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class MobileEntranceGetAllArguments : IArgumentsBase
    {
		public int? PaymentConcessionId;

		#region Constructor
		public MobileEntranceGetAllArguments(int? paymentConcessionId)
		{
			PaymentConcessionId = paymentConcessionId;
		}
		#endregion Constructor
	}
}
