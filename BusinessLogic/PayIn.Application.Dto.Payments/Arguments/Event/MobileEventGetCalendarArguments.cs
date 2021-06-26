using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class MobileEventGetCalendarArguments : IArgumentsBase
	{
		public int? PaymentConcessionId { get; set; }

		#region Constructors
		public MobileEventGetCalendarArguments(int? paymentConcessionId)
		{
			PaymentConcessionId = paymentConcessionId;
		}
		#endregion Constructors
	}
}
