using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class LiquidationGetPaymentConcessionSelectorArguments : IArgumentsBase
	{
		public string Filter { get; private set; }

		#region Constructors
		public LiquidationGetPaymentConcessionSelectorArguments(string filter)
		{
			Filter = filter;
		}
		#endregion Constructors
	}
}

