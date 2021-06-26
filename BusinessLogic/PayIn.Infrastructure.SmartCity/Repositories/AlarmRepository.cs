using PayIn.BusinessLogic.Common;
using PayIn.Domain.SmartCity;
using PayIn.Infrastructure.SmartCity.Db;
using System;

namespace PayIn.Infrastructure.SmartCity.Repositories
{
	public class AlarmRepository : SmartCityRepository<Alarm>
	{
		public readonly ISessionData SessionData;

		#region Constructors
		public AlarmRepository(
			ISessionData sessionData,
			ISmartCityContext context
		)
			: base(context)
		{
			SessionData = sessionData ?? throw new ArgumentNullException("sessionData");
		}
		#endregion Constructors
	}
}
