using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Payments.Repositories
{
	public class TicketDetailRepository : PublicRepository<TicketDetail>
	{
		#region Constructors
		public TicketDetailRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Constructors
	}
}
