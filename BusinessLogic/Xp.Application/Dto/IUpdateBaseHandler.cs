using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;
using Xp.Domain;

namespace Xp.Application.Dto
{
	public interface IUpdateBaseHandler<TArguments, TEntity>
		where TArguments : IUpdateArgumentsBase<TEntity>
		where TEntity : IEntity
	{
		Task<dynamic> ExecuteAsync(TArguments arguments);
	}
	public interface IUpdateBaseHandler<TArguments, TEntity, TResult>
		where TArguments : IUpdateArgumentsBase<TEntity>
		where TEntity : IEntity
	{
		Task<TResult> ExecuteAsync(TArguments arguments);
	}
}
