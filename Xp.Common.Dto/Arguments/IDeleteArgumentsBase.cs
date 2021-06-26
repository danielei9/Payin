using Xp.Domain;

namespace Xp.Common.Dto.Arguments
{
	public interface IDeleteArgumentsBase<TEntity> : IArgumentsBase
		where TEntity : IEntity
	{
		int Id { get; }
	}
}
