using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ShipmentRepository : PublicRepository<Shipment>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ShipmentRepository(
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
		public override IQueryable<Shipment> CheckHorizontalVisibility(IQueryable<Shipment> that)
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
