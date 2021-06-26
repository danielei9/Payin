using System.Linq;
using System.Threading.Tasks;

namespace Xp.Domain
{
	public interface IQueueRepository<TEntity>
		where TEntity : IQueueEntity
	{
		Task PushAsync(IQueueEntity entity);

		Task<TResult> PopAsync<TResult>()
			where TResult : IQueueEntity;

		Task<TResult> PeekAsync<TResult>()
			where TResult : IQueueEntity;

		Task DeleteAsync(IQueueEntity entity);
		//Task DeleteAsync<TResult>(TResult entity)
		//	where TResult : IQueueEntity;
	}
}
