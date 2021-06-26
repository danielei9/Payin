using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketMobileUpdateStateAndGetOrdersArguments : TicketMobileUpdateStateArguments, IArgumentsBase
	{
		#region Constructor
		public TicketMobileUpdateStateAndGetOrdersArguments(TicketStateType state)
			:base(state)
		{
		}
		#endregion Constructor
	}
}
