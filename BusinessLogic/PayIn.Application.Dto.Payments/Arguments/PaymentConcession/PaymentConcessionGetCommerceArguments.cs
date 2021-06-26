using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentConcessionGetCommerceArguments: IArgumentsBase
	{
		public int Id { get; private set; }

		#region Constructors
		public PaymentConcessionGetCommerceArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
