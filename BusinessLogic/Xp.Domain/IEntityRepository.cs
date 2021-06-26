using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xp.Domain
{
	public interface IEntityRepository<T> : IDisposable
		where T : IEntity
	{
		Task<IQueryable<T>> GetAsync(params string[] includes);
		Task<T> GetAsync(int id, params string[] includes);
		Task<int> AddAsync(T entity);
		Task DeleteAsync(T entity);
		Task DeleteAsync(IEnumerable<T> entities);
	}
}
