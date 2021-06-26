using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentConcessionMobileGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public PaymentConcessionMobileGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
