using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments.Repositories
{
	public class PaymentRepository : PublicRepository<Payment>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public PaymentRepository(
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
		public override IQueryable<Payment> CheckHorizontalVisibility(IQueryable<Payment> that)
		{
			var result = that;
			if (SessionData.Login.IsNullOrEmpty()) // Para llamadas desde el Sabadell sin autenticar
			{
				result = result
					.Where(x =>
						x.State == PaymentState.Pending ||
						x.LiquidationId == null
					);
			}

			return result;
		}
		#endregion CheckHorizontalVisibility
	}
}
