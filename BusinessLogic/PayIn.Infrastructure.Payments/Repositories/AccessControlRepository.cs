using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments
{
    class AccessControlRepository : PublicRepository<AccessControl>
	{
		public readonly ISessionData SessionData;

		#region Contructors

		public AccessControlRepository(
			ISessionData sessionData,
			IPublicContext context
		)
			: base(context)
		{
            SessionData = sessionData ?? throw new ArgumentNullException(nameof(sessionData));
		}

		#endregion

		#region CheckHorizontalVisibility

		public override IQueryable<AccessControl> CheckHorizontalVisibility(IQueryable<AccessControl> that)
		{
			return that
				.Where(x =>
					x.PaymentConcession.Concession.State != ConcessionState.Removed
				);
		}

		#endregion
	}
}
