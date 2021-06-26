using Xp.Domain;

namespace Xp.Common.Dto.Arguments
{
	public interface ICreateArgumentsBase<TEntity> : IArgumentsBase
		where TEntity : IEntity
	{
	}
}
