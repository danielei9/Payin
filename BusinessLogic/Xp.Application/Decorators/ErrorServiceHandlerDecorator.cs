using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common.Dto.Arguments;

namespace Xp.Application.Decorators
{
	public class ErrorServiceHandlerDecorator<TQuery> : ErrorHandlerDecorator,
		IServiceBaseHandler<TQuery>
		where TQuery : IArgumentsBase
	{
		private readonly IServiceBaseHandler<TQuery> Handler;

		#region Contructors
		public ErrorServiceHandlerDecorator(IServiceBaseHandler<TQuery> handler)
		{
			if (handler == null)
				throw new ArgumentNullException("handler");
			Handler = handler;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TQuery query)
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
