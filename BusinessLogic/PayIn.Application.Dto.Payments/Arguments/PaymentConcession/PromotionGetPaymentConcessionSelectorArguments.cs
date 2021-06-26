using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class PromotionGetPaymentConcessionSelectorArguments : IArgumentsBase
	{
		public string Filter { get; private set; }

		#region Constructors
		public PromotionGetPaymentConcessionSelectorArguments(string filter)
		{
			Filter = filter;
		}
		#endregion Constructors
	}
}

