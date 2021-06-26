using PayIn.BusinessLogic.Common;
using PayIn.Domain.SmartCity;
using PayIn.Infrastructure.SmartCity.Db;
using System;

namespace PayIn.Infrastructure.SmartCity.Repositories
{
	public class EnergyHolidayRepository : SmartCityRepository<EnergyHoliday>
	{
		public readonly ISessionData SessionData;

		#region Constructors
		public EnergyHolidayRepository(
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
