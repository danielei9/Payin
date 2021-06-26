using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments
{
	public class EventRepository : PublicRepository<Event>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public EventRepository(
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
		public override IQueryable<Event> CheckHorizontalVisibility(IQueryable<Event> that)
		{
			return that
				.Where(x =>
					x.State != EventState.Deleted &&
					x.PaymentConcession.Concession.State != ConcessionState.Removed &&
					(
						SessionData.Roles.Contains(AccountRoles.Superadministrator) ||
						x.PaymentConcession.Concession.Supplier.Login == SessionData.Login ||
						x.PaymentConcession.PaymentWorkers
							.Any(y => y.Login == SessionData.Login) ||
						x.Visibility != EventVisibility.Members ||
						x.PaymentConcession.Concession.Cards
							.Any(y => y.Users
								.Any(z => z.Login == SessionData.Login)
							)
					)
				);
		}
		#endregion CheckHorizontalVisibility
	}
}
