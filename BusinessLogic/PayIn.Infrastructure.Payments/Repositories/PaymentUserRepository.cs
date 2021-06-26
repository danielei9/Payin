using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class PaymentUserRepository : PublicRepository<PaymentUser>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public PaymentUserRepository(
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
		public override IQueryable<PaymentUser> CheckHorizontalVisibility(IQueryable<PaymentUser> that)
		{
			that = that
				.Where(x =>
					x.Concession.Concession.State == ConcessionState.Active && (
						x.Concession.Concession.Supplier.Login == SessionData.Login ||
						x.Concession.PaymentUsers.Select(y => y.Login).Contains(SessionData.Login)
				));
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
