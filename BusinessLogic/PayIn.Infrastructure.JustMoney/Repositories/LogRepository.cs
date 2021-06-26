using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using PayIn.Infrastructure.JustMoney.Db;
using System;

namespace PayIn.Infrastructure.JustMoney.Repositories
{
	public class LogRepository : JustMoneyRepository<Log>
	{
		public readonly ISessionData SessionData;

		#region Constructors
		public LogRepository(
			ISessionData sessionData,
			IJustMoneyContext context
		)
			: base(context)
		{
			SessionData = sessionData ?? throw new ArgumentNullException("sessionData");
		}
		#endregion Constructors
	}
}
