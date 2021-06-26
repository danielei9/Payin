using PayIn.Infrastructure.SmartCity.Db;
using Xp.Infrastructure;

namespace PayIn.Infrastructure.SmartCity
{
	public class SmartCityUnitOrWork : UnitOfWork<ISmartCityContext>
	{
		#region Constructors
		public SmartCityUnitOrWork(ISmartCityContext context)
			: base(context)
		{
		}
		#endregion Constructors
	}
}
