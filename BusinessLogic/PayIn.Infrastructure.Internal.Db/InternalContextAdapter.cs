using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xp.Domain;

namespace PayIn.Infrastructure.Internal.Db
{
	public class InternalContextAdapter : IInternalContext
	{
		private DbContextTransaction Transaction;
		private readonly InternalContext Context;

		#region Constructors
		public InternalContextAdapter(InternalContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");
			Context = context;
		}
		#endregion Constructors

		#region Initialize
		public void Initialize()
		{
			Context.Initialize();
		}
		#endregion Initialize

		#region Set
		public IQueryable<TEntity> Set<TEntity>(string[] includes)
			where TEntity : class, IEntity
		{
			IQueryable<TEntity> result = Context.Set<TEntity>();
			foreach (var include in includes)
				result = result.Include(include);

			return result;
		}
		#endregion Set

		#region Add
		public void Add<TEntity>(TEntity entity)
			where TEntity : class, IEntity
		{
			Context
				.Set<TEntity>()
				.Add(entity);
		}
		#endregion Add

		#region Delete
		public void Delete<TEntity>(TEntity entity)
			where TEntity : class, IEntity
		{
			Context
				.Set<TEntity>()
				.Remove(entity);
		}
		#endregion Delete

		#region SaveAsync
		public async Task SaveAsync()
		{
			await Context.SaveChangesAsync();
		}
		#endregion SaveAsync

		#region Dispose
		public void Dispose()
		{
			Context.Dispose();
		}
		#endregion Dispose

		#region BeginTransaction
		public void BeginTransaction()
		{
			Transaction = Context.Database.BeginTransaction();
		}
		#endregion BeginTransaction

		#region CommitTransaction
		public void CommitTransaction()
		{
			if (Transaction != null)
				Transaction.Commit();
			Transaction = null;
		}
		#endregion CommitTransaction

		#region RollbackTransaction
		public void RollbackTransaction()
		{
			if (Transaction != null)
				Transaction.Rollback();
			Transaction = null;
		}
		#endregion RollbackTransaction
	}
}
