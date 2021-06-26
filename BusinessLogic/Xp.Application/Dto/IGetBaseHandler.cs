using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace Xp.Application.Dto
{
	public interface IGetBaseHandler<TArguments, TResult>
		where TArguments : IArgumentsBase
	{
		Task<TResult> ExecuteAsync(TArguments query);
	}
}
