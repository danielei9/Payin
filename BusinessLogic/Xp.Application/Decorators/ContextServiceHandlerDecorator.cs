using System;
using System.Threading.Tasks;
using System.Transactions;
using Xp.Application.Dto;
using Xp.Common.Dto.Arguments;
using Xp.Domain;

namespace Xp.Application.Decorators
{
	public class ContextServiceHandlerDecorator<TQuery> : IServiceBaseHandler<TQuery>
		where TQuery : IArgumentsBase
	{
		private readonly IServiceBaseHandler<TQuery> Handler;
		private readonly IUnitOfWork UnitOfWork;

		#region Contructors
		public ContextServiceHandlerDecorator(IServiceBaseHandler<TQuery> handler, IUnitOfWork unitOfWork)
		{
			if (handler == null)
				throw new ArgumentNullException("handler");
			Handler = handler;

			if (unitOfWork == null)
				throw new ArgumentNullException("unitOfWork");
			UnitOfWork = unitOfWork;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TQuery query)
		{
			try
			{
				//UnitOfWork.BeginTransaction();

				var result = await Handler.ExecuteAsync(query);
				await UnitOfWork.SaveAsync();

				//UnitOfWork.CommitTransaction();
				
				return result;
			}
			catch (Exception ex)
			{
				//UnitOfWork.RollbackTransaction();
				throw;
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
