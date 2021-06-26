using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common.Dto.Arguments;

namespace Xp.Application.Handlers
{
	public abstract class ScriptHandler<TArguments, RArguments> : IQueryBaseHandler<TArguments, RArguments>
		where TArguments : IArgumentsBase
	{
		public abstract Task<ResultBase<RArguments>> ExecuteAsync(TArguments arguments);
	}
}
