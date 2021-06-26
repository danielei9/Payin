using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments
{
    class AccessControlEntranceRepository : PublicRepository<AccessControlEntrance>
	{
		public readonly ISessionData SessionData;

		#region Contructors

		public AccessControlEntranceRepository(
			ISessionData sessionData,
			IPublicContext context
		)
			: base(context)
		{
            SessionData = sessionData ?? throw new ArgumentNullException(nameof(sessionData));
		}

		#endregion

		#region CheckHorizontalVisibility

		public override IQueryable<AccessControlEntrance> CheckHorizontalVisibility(IQueryable<AccessControlEntrance> that)
		{
			return that
				.Where(x =>
					x.AccessControl.PaymentConcession.Concession.State != ConcessionState.Removed
				);
		}

		#endregion
	}
}
