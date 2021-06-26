using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.PaymentUser
{
	public class PaymentUserDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructor
		public PaymentUserDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
