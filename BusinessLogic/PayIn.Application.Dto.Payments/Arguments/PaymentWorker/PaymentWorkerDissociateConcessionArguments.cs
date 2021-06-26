using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentWorkerDissociateConcessionArguments  : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public PaymentWorkerDissociateConcessionArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
