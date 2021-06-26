using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Payment
{
	public class UnliquidatedPaymentArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public UnliquidatedPaymentArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}