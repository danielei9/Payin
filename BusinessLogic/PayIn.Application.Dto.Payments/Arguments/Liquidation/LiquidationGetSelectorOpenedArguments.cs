using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class LiquidationGetSelectorOpenedArguments : IArgumentsBase
	{
        public int? PaymentConcessionId { get; private set; }
		public string Filter { get; private set; }

		#region Constructors
		public LiquidationGetSelectorOpenedArguments(string filter, int? paymentConcessionId)
		{
			Filter = filter;
            PaymentConcessionId = paymentConcessionId;

        }
		#endregion Constructors
	}
}

