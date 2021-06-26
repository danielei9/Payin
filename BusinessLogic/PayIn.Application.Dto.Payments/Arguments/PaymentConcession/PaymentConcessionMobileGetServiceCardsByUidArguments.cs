using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentConcessionMobileGetServiceCardsByUidArguments : IArgumentsBase
	{
		public long Uid { get; set; }

		#region Constructors
		public PaymentConcessionMobileGetServiceCardsByUidArguments(long uid)
		{
			Uid = uid;
		}
		#endregion Constructors
	}
}
