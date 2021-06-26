using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common.Dto.Arguments;

namespace Xp.Application.Decorators
{
	public class ErrorQueryHandlerDecorator<TQuery, TResult> : ErrorHandlerDecorator,
		IQueryBaseHandler<TQuery, TResult>
		where TQuery : IArgumentsBase
	{
		private readonly IQueryBaseHandler<TQuery, TResult> Handler;

		#region Contructors
		public ErrorQueryHandlerDecorator(IQueryBaseHandler<TQuery, TResult> handler)
		{
			//Handler = handler ?? throw new ArgumentNullException("handler");
			if (handler == null)
				throw new ArgumentNullException("handler");
			Handler = handler;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<ResultBase<TResult>> ExecuteAsync(TQuery query)
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
