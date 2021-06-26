using PayIn.Common;
using PayIn.Domain.Payments;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketMobileUpdateStateArguments : IUpdateArgumentsBase<Ticket>
	{
		           public int             Id    { get; set; }
		[Required] public TicketStateType State { get; set; }

		#region Constructor
		public TicketMobileUpdateStateArguments(TicketStateType state)
		{
			State = state;
		}
		#endregion Constructor
	}
}
