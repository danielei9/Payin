using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common.Dto.Arguments;

namespace Xp.Application.Decorators
{
	public class ErrorGetHandlerDecorator<TQuery, TResult> : ErrorHandlerDecorator,
		IGetBaseHandler<TQuery, TResult>
		where TQuery : IArgumentsBase
	{
		private readonly IGetBaseHandler<TQuery, TResult> Handler;

		#region Contructors
		public ErrorGetHandlerDecorator(IGetBaseHandler<TQuery, TResult> handler)
		{
			if (handler == null)
				throw new ArgumentNullException("handler");
			Handler = handler;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<TResult> ExecuteAsync(TQuery query)
		{
			try
			{
				var result = await Handler.ExecuteAsync(query);
				return result;
			}
			catch (Exception ex)
			{
				var exception = FilterException(ex);
				if (exception != null)
					throw exception;

				throw;
			}
		}
		#endregion ExecuteAsync
	}
}
