using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Internal.Arguments
{
	public class PaymentMediaGetBalanceArguments : IArgumentsBase
	{
		public int PublicId { get; set; }

		#region Constructors
		public PaymentMediaGetBalanceArguments()
		{
		}

		public PaymentMediaGetBalanceArguments(int publicId)
		{
			PublicId = publicId;
		}
		#endregion Constructors
	}
}
