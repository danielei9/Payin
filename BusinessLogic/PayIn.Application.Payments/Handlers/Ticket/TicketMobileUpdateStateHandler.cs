using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using Xp.Application.Attributes;
using Xp.Application.Handlers;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Ticket", "UpdateState")]
	public class TicketMobileUpdateStateHandler :
		UpdateHandler<TicketMobileUpdateStateArguments,Ticket>
	{
		#region Contructors
		public TicketMobileUpdateStateHandler(
            IEntityRepository<Ticket> repository
		)
			: base(repository)
		{
		}
		#endregion Contructors
	}
}
