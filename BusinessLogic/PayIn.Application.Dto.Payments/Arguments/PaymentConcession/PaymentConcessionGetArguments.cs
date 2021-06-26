using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class PaymentConcessionGetArguments : IArgumentsBase
	{
		public int Id { get; private set; }

		#region Constructors
		public PaymentConcessionGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
