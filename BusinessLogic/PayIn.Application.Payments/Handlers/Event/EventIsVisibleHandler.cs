using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using Xp.Application.Handlers;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class EventIsVisibleHandler :
		UpdateHandler<EventIsVisibleArguments, Event>
	{
		#region Constructors
		public EventIsVisibleHandler(IEntityRepository<Event> repository)
			: base(repository)
		{
		}
		#endregion Constructors
	}
}