using System.Threading.Tasks;
using System.Windows.Input;

namespace Xp.Presentation.Common.Commands
{
	public interface IAsyncCommand : ICommand
	{
		Task ExecuteAsync(object parameter);
	}
	public interface IAsyncCommand<TResult> : ICommand
	{
		Task<TResult> ExecuteAsync(object parameter);
	}
}
