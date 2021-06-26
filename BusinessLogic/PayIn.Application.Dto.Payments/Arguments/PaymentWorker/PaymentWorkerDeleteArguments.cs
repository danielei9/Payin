using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentWorkerDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public PaymentWorkerDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}

