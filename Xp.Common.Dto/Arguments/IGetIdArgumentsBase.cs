using Xp.Domain;

namespace Xp.Common.Dto.Arguments
{
	public interface IGetIdArgumentsBase<TResult, TEntity> : IGetArgumentsBase<TEntity>
		where TEntity : IEntity
	{
		int Id { get; }
	}
}
