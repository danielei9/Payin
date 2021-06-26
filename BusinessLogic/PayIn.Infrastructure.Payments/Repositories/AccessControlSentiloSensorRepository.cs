using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments
{
    class AccessControlSentiloSensorRepository : PublicRepository<AccessControlSentiloSensor>
	{
		public readonly ISessionData SessionData;

		#region Contructors

		public AccessControlSentiloSensorRepository(
			ISessionData sessionData,
			IPublicContext context
		)
			: base(context)
		{
            SessionData = sessionData ?? throw new ArgumentNullException(nameof(sessionData));
		}

		#endregion

		#region CheckHorizontalVisibility

		public override IQueryable<AccessControlSentiloSensor> CheckHorizontalVisibility(IQueryable<AccessControlSentiloSensor> that)
		{
			return that;
		}

		#endregion
	}
}
