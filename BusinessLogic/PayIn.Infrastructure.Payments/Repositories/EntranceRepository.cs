using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments
{
	public class EntranceRepository : PublicRepository<Entrance>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public EntranceRepository(
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
		public override IQueryable<Entrance> CheckHorizontalVisibility(IQueryable<Entrance> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
