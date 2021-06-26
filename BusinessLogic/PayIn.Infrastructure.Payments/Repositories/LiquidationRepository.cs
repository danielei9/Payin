using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;
using PayIn.Domain.Security;

namespace PayIn.Infrastructure.Payments.Repositories
{
	public class LiquidationRepository : PublicRepository<Liquidation>
	{
		public readonly ISessionData SessionData;

		#region Constructors
		public LiquidationRepository(
			ISessionData sessionData, 
			IPublicContext context
		)
			: base(context)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region CheckHorizontalVisibility
		public override IQueryable<Liquidation> CheckHorizontalVisibility(IQueryable<Liquidation> that)
		{
			var result = that
				.Where(x =>
					(x.Concession.Concession.Type == ServiceType.Charge)
					// &&((x.Payments.Any(y => y.Ticket.Concession.Concession.Supplier.Login == SessionData.Login)
                )
				;
			return result;
		}
		#endregion CheckHorizontalVisibility
	}
}

