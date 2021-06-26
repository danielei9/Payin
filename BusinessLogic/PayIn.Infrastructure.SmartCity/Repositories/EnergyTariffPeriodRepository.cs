using PayIn.BusinessLogic.Common;
using PayIn.Domain.SmartCity;
using PayIn.Infrastructure.SmartCity.Db;
using System;

namespace PayIn.Infrastructure.SmartCity.Repositories
{
	public class EnergyTariffPeriodRepository : SmartCityRepository<EnergyTariffPeriod>
	{
		public readonly ISessionData SessionData;

		#region Constructors
		public EnergyTariffPeriodRepository(
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
