using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments
{
	public class CheckRepository : PublicRepository<Check>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public CheckRepository(
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
		public override IQueryable<Check> CheckHorizontalVisibility(IQueryable<Check> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
