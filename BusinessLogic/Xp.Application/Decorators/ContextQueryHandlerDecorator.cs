using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common.Dto.Arguments;
using Xp.Domain;

namespace Xp.Application.Decorators
{
	public class ContextQueryHandlerDecorator<TArguments, TResult> : IQueryBaseHandler<TArguments, TResult>
		where TArguments : IArgumentsBase
	{
		private readonly IQueryBaseHandler<TArguments, TResult> Handler;
		private readonly IUnitOfWork UnitOfWork;

		#region Contructors
		public ContextQueryHandlerDecorator(IQueryBaseHandler<TArguments, TResult> handler, IUnitOfWork unitOfWork)
		{
			if (handler == null) throw new ArgumentNullException("handler");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");

			Handler = handler;
			UnitOfWork = unitOfWork;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<ResultBase<TResult>> ExecuteAsync(TArguments query)
		{
			try
			{
				var result = await Handler.ExecuteAsync(query);
				await UnitOfWork.SaveAsync();
				return result;
			}
			finally
			{
				Dispose();
			}
		}
		#endregion ExecuteAsync

		#region Dispose
		public void Dispose()
		{
			if (UnitOfWork != null)
				UnitOfWork.Dispose();
		}
		#endregion Dispose
	}
}
