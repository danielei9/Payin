using PayIn.BusinessLogic.Common;
using PayIn.Domain.SmartCity;
using PayIn.Infrastructure.SmartCity.Db;
using System;

namespace PayIn.Infrastructure.SmartCity.Repositories
{
	public class EnergyContractRepository : SmartCityRepository<EnergyContract>
	{
		public readonly ISessionData SessionData;

		#region Constructors
		public EnergyContractRepository(
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
