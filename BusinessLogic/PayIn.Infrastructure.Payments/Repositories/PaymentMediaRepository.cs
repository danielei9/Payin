using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments.Repositories
{
	public class PaymentMediaRepository : PublicRepository<PaymentMedia>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public PaymentMediaRepository(
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
		public override IQueryable<PaymentMedia> CheckHorizontalVisibility(IQueryable<PaymentMedia> that)
		{
			var result = that;
			if (SessionData.Login.IsNullOrEmpty())
			{
				result = result
					.Where(x =>
						x.State == PaymentMediaState.Pending
					);
			}
			else
			{
				result = result
					.Where(x =>
						x.Login == SessionData.Login ||
						x.PaymentConcession.Concession.Supplier.Login == SessionData.Login
					);
			}
			return result;
		}
		#endregion CheckHorizontalVisibility
	}
}
