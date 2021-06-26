using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace Xp.Application.Dto
{
	public interface IServiceBaseHandler<TArguments>
		where TArguments : IArgumentsBase
	{
		Task<dynamic> ExecuteAsync(TArguments arguments);
	}
}
