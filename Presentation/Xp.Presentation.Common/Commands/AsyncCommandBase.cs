using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Xp.Presentation.Common.Commands
{
	public abstract class AsyncCommandBase : IAsyncCommand
	{
		#region CanExecuteChanged
		public event EventHandler CanExecuteChanged;
		protected void OnCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
				CanExecuteChanged(this, new EventArgs());
		}
		#endregion CanExecuteChanged

		#region CanExecute
		public virtual bool CanExecute(object parameter)
		{
			return true;
		}
		#endregion CanExecute

		#region ExecuteAsync
		public abstract Task ExecuteAsync(object parameter);
		#endregion ExecuteAsync

		#region Execute
		async void ICommand.Execute(object parameter)
		{
			await ExecuteAsync(parameter);
		}
		#endregion Execute
	}
	public abstract class AsyncCommandBase<TResult> : IAsyncCommand<TResult>
	{
		#region CanExecuteChanged
		public event EventHandler CanExecuteChanged;
		protected void OnCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
				CanExecuteChanged(this, new EventArgs());
		}
		#endregion CanExecuteChanged

		#region CanExecute
		public virtual bool CanExecute(object parameter)
		{
			return true;
		}
		#endregion CanExecute

		#region ExecuteAsync
		public abstract Task<TResult> ExecuteAsync(object parameter);
		#endregion ExecuteAsync

		#region Execute
		async void ICommand.Execute(object parameter)
		{
			await ExecuteAsync(parameter);
		}
		#endregion Execute
	}
}
