using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;

namespace PayIn.Infrastructure.Payments
{
	public class EntranceSystemRepository : PublicRepository<EntranceSystem>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public EntranceSystemRepository(
			ISessionData sessionData,
			IPublicContext context
		)
			: base(context)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Contructors
	}
}
