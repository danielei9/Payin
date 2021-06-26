using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments.Repositories
{
	public class PaymentWorkerRepository : PublicRepository<PaymentWorker>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public PaymentWorkerRepository(
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
		public override IQueryable<PaymentWorker> CheckHorizontalVisibility(IQueryable<PaymentWorker> result)
		{
			result = result
				.Where(x => 
					x.Concession.Concession.State == ConcessionState.Active && (
						x.Concession.Concession.Supplier.Login == SessionData.Login ||
						x.Concession.PaymentWorkers.Select(y => y.Login).Contains(SessionData.Login)
				));

			return result;
		}
		#endregion CheckHorizontalVisibility
	}
}