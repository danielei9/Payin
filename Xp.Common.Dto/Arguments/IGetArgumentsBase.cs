using Xp.Domain;

namespace Xp.Common.Dto.Arguments
{
	public interface IGetArgumentsBase<TEntity> : IArgumentsBase
		where TEntity : IEntity
	{
	}
}
