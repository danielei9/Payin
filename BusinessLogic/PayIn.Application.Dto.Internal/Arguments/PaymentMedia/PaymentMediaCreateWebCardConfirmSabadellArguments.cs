using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Internal.Arguments
{
	public class PaymentMediaCreateWebCardConfirmSabadellArguments : IArgumentsBase
	{
		public int PublicPaymentMediaId { get; set; }

		#region Constructors
		public PaymentMediaCreateWebCardConfirmSabadellArguments(int publicPaymentMediaId)
		{
			PublicPaymentMediaId = publicPaymentMediaId;
		}
		#endregion Constructors
	}
}
