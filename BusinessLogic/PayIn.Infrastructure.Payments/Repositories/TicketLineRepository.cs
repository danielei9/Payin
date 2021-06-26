using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class TicketLineRepository : PublicRepository<TicketLine>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public TicketLineRepository(
			ISessionData sessionData,
			IPublicContext context
		)
			: base(context)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<TicketLine> CheckHorizontalVisibility(IQueryable<TicketLine> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
