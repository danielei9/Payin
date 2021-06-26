using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Infrastructure.Public.Db
{
	public abstract class PublicRepository<TEntity> : Repository<TEntity, IPublicContext>
		where TEntity : class, IEntity, new()
	{
		#region Contructors
		public PublicRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
