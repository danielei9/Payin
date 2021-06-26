using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentWorkerResendNotificationArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public PaymentWorkerResendNotificationArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
