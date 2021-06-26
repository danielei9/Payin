using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Domain;

namespace Xp.Infrastructure
{
	public abstract class Repository<TEntity, TContext> : IEntityRepository<TEntity>
		where TEntity : class, IEntity, new()
		where TContext : IContext
	{
		protected readonly TContext Context;

		#region Contructors
		public Repository(TContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");
			Context = context;
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public virtual IQueryable<TEntity> CheckHorizontalVisibility(IQueryable<TEntity> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility

		#region GetAsync
		public async virtual Task<IQueryable<TEntity>> GetAsync(params string[] includes)
		{
			return await Task.Run(() =>
			{
				var result = Context.Set<TEntity>(includes);
				return CheckHorizontalVisibility(result);
			});
		}
		public async virtual Task<TEntity> GetAsync(int id, params string[] includes)
		{
			var result = await GetAsync(includes);
			return result
				.Where(x => x.Id == id)
				.FirstOrDefault();
		}
		#endregion GetAsync

		#region AddAsync
		public async virtual Task<int> AddAsync(TEntity entity)
		{
			return await Task.Run(() =>
			{
				Context.Add(entity);
				return entity.Id;
			});
		}
		#endregion AddAsync

		#region DeleteAsync
		public async virtual Task DeleteAsync(TEntity entity)
		{
			await Task.Run(() =>
			{
				Context.Delete(entity);
			});
		}
		public async virtual Task DeleteAsync(IEnumerable<TEntity> entities)
		{
			await Task.Run(() =>
			{
				foreach (var entity in entities)
					Context.Delete(entity);
			});
		}
		#endregion DeleteAsync

		#region Dispose
		public virtual void Dispose()
		{
			if (Context != null)
				Context.Dispose();
		}
		#endregion Dispose
	}
}
