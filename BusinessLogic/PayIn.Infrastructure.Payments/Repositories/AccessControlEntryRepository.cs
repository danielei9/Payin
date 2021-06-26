using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments
{
    class AccessControlEntryRepository : PublicRepository<AccessControlEntry>
	{
		public readonly ISessionData SessionData;

		#region Contructors

		public AccessControlEntryRepository(
			ISessionData sessionData,
			IPublicContext context
		)
			: base(context)
		{
            SessionData = sessionData ?? throw new ArgumentNullException(nameof(sessionData));
		}

		#endregion

		#region CheckHorizontalVisibility

		public override IQueryable<AccessControlEntry> CheckHorizontalVisibility(IQueryable<AccessControlEntry> that)
		{
			return that;
		}

		#endregion
	}
}
