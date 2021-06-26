using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentConcessionPurseDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructor
		public PaymentConcessionPurseDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
