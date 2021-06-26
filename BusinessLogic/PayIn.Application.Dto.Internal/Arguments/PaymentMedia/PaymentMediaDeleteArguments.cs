using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Internal.Arguments
{
	public class PaymentMediaDeleteArguments : IArgumentsBase
	{
		public int PublicId { get; set; }

		#region Constructors
		public PaymentMediaDeleteArguments()
		{
		}

		public PaymentMediaDeleteArguments(int publicId)
		{
			PublicId = publicId;
		}
		#endregion Constructors
	}
}
