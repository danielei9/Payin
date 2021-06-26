using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Internal.Arguments
{
	public class PaymentMediaGetBalanceToRefundArguments : IArgumentsBase
	{
		public int PublicId { get; set; }
	

		#region Constructors
		public PaymentMediaGetBalanceToRefundArguments()
		{
		}

		public PaymentMediaGetBalanceToRefundArguments(int publicId)
		{
			PublicId = publicId;			
		}
		#endregion Constructors
	}
}
