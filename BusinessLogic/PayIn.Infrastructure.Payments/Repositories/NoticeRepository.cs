using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Infrastructure.Payments
{
	public class NoticeRepository : PublicRepository<Notice>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public NoticeRepository(
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
		public override IQueryable<Notice> CheckHorizontalVisibility(IQueryable<Notice> that)
		{
			return that
				.Where(x =>
					x.Event.State != EventState.Deleted &&
					x.State != NoticeState.Deleted &&
					x.Event.PaymentConcession.Concession.State != ConcessionState.Removed &&
					(
						SessionData.Roles.Contains(AccountRoles.Superadministrator) ||
						x.Event.PaymentConcession.Concession.Supplier.Login == SessionData.Login ||
						x.Event.PaymentConcession.PaymentWorkers
							.Any(y => y.Login == SessionData.Login) ||
						x.Visibility != NoticeVisibility.Members ||
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
