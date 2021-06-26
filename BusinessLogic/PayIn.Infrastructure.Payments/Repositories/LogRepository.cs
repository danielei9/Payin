using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments
{
	public class LogRepository : PublicRepository<Log>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public LogRepository(
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
		public override IQueryable<Log> CheckHorizontalVisibility(IQueryable<Log> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
