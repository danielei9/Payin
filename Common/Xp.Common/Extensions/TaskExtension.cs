using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Threading.Tasks
{
	public static class TaskExtension
	{
		#region WaitWithResult
		public static TResult WaitWithResult<TResult>(this Task<TResult> that)
		{
			//that.Wait();
			Task.WaitAll(that);

			return that.Result;
		}
		#endregion WaitWithResult

		#region ContinueSuccessWith
		public static Task<TResult> ContinueSuccessWith<TResult>(this Task<TResult> that, Action<TResult> result)
		{
			that.ContinueWith(x =>
				{
					if (x.Status == TaskStatus.RanToCompletion)
						result(x.Result);
				});

			return that;
		}
		#endregion ContinueSuccessWith

		#region ContinueExceptionWith
		public static Task ContinueExceptionWith(this Task that, Action<Exception> result)
		{
			that.ContinueWith(x =>
				{
					if (x.Status == TaskStatus.Faulted)
						result(x.Exception);
				});

			return that;
		}
		#endregion ContinueExceptionWith
	}
}
