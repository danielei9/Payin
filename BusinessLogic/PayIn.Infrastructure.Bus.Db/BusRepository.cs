using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Infrastructure.Bus.Db
{
	public abstract class BusRepository<TEntity> : Repository<TEntity, IBusContext>
		where TEntity : class, IEntity, new()
	{
		#region Contructors
		public BusRepository(IBusContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
