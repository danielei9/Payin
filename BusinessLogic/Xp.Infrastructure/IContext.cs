using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Domain;

namespace Xp.Infrastructure
{
	public interface IContext : IDisposable
	{
		IQueryable<TEntity> Set<TEntity>(params string[] includes)
			where TEntity : class, IEntity;

		void Add<TEntity>(TEntity entity)
			where TEntity : class, IEntity;
		void Delete<TEntity>(TEntity entity)
			where TEntity : class, IEntity;

		Task SaveAsync();

		void BeginTransaction();
		void CommitTransaction();
		void RollbackTransaction();
	}
}
