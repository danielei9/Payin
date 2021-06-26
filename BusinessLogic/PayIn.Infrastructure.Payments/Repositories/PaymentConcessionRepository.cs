using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments.Repositories
{
	public class PaymentConcessionRepository : PublicRepository<PaymentConcession>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public PaymentConcessionRepository(
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
		public override IQueryable<PaymentConcession> CheckHorizontalVisibility(IQueryable<PaymentConcession> that)
		{
			var result = that
				.Where(x =>
					x.Concession.Type == ServiceType.Charge
				);

			return result;
		}
		#endregion CheckHorizontalVisibility
	}
}
