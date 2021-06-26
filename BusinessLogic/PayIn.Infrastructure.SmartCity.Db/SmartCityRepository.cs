using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Infrastructure.SmartCity.Db
{
	public abstract class SmartCityRepository<TEntity> : Repository<TEntity, ISmartCityContext>
		where TEntity : class, IEntity, new()
	{
		#region Contructors
		public SmartCityRepository(ISmartCityContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
