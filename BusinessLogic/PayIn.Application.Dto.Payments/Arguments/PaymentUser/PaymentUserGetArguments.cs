using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.PaymentUser
{
	public partial class PaymentUserGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public PaymentUserGetArguments(int id)
		{
			Id = id;
		}
		public PaymentUserGetArguments()
		{
		}
		#endregion Constructors
	}
}
