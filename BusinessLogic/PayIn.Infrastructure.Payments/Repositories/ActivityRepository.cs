using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments
{
	class ActivityRepository : PublicRepository<Activity>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ActivityRepository(
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
		public override IQueryable<Activity> CheckHorizontalVisibility(IQueryable<Activity> that)
		{
			return that
				.Where(x =>
					x.Event.PaymentConcession.Concession.State != ConcessionState.Removed &&
					(
						SessionData.Roles.Contains(AccountRoles.Superadministrator) ||
						x.Event.PaymentConcession.Concession.Supplier.Login == SessionData.Login ||
						x.Event.PaymentConcession.PaymentWorkers
							.Any(y => y.Login == SessionData.Login) ||
						x.Event.Visibility != EventVisibility.Members ||
						x.Event.PaymentConcession.Concession.Cards
							.Any(y => y.Users
								.Any(z => z.Login == SessionData.Login)
							)
					)
				);
		}
		#endregion CheckHorizontalVisibility
	}
}
