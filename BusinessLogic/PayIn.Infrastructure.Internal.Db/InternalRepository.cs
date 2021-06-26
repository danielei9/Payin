using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Infrastructure.Internal.Db
{
	public abstract class InternalRepository<TEntity> : Repository<TEntity, IInternalContext>
		where TEntity : class, IEntity, new()
	{
		#region Contructors
		public InternalRepository(IInternalContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
