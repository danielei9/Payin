using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments
{
	public class EntranceTypeRepository : PublicRepository<EntranceType>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public EntranceTypeRepository(
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
		public override IQueryable<EntranceType> CheckHorizontalVisibility(IQueryable<EntranceType> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
