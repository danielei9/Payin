using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace Xp.Application.Dto
{
	public interface IQueryBaseHandler<TArguments, TResult>
		where TArguments : IArgumentsBase
	{
		Task<ResultBase<TResult>> ExecuteAsync(TArguments arguments);
	}
}
