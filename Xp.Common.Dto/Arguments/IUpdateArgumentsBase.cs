using Xp.Domain;

namespace Xp.Common.Dto.Arguments
{
	public interface IUpdateArgumentsBase<TEntity> : IArgumentsBase
		where TEntity : IEntity
	{
		int Id { get; }
	}
}
