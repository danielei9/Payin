using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobilePaymentMediaDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public MobilePaymentMediaDeleteArguments()
		{
		}

		public MobilePaymentMediaDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
