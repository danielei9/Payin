using System;
using System.Threading.Tasks;

namespace Xp.Domain
{
	public interface IUnitOfWork : IDisposable
	{
		Task SaveAsync();

		void BeginTransaction();
		void CommitTransaction();

		void RollbackTransaction();
	}

	public interface IUnitOfWork<T> : IUnitOfWork
	{
	}
}
