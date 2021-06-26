using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Infrastructure.JustMoney.Db
{
	public abstract class JustMoneyRepository<TEntity> : Repository<TEntity, IJustMoneyContext>
		where TEntity : class, IEntity, new()
	{
		#region Contructors
		public JustMoneyRepository(IJustMoneyContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
