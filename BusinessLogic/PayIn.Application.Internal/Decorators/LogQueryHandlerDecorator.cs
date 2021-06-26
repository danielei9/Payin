using PayIn.BusinessLogic.Common;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common.Dto.Arguments;
using Xp.Domain;

namespace PayIn.Application.Internal.Decorators
{
	public class LogQueryHandlerDecorator<TArguments, TResult> : LogHandlerDecorator, IQueryBaseHandler<TArguments, TResult>
		where TArguments : IArgumentsBase
	{
		public IQueryBaseHandler<TArguments, TResult> Handler { get; private set; }
		private readonly IUnitOfWork UnitOfWork;

		#region Contructors
		public LogQueryHandlerDecorator(
			IQueryBaseHandler<TArguments, TResult> handler,
			ISessionData sessionData,
			IUnitOfWork unitOfWork,
			string relatedClass,
			string relatedMethod,
			string relatedId
		)
			: base(sessionData, relatedClass, relatedMethod, relatedId)
		{
			if (handler == null) throw new ArgumentNullException("handler");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");

			Handler = handler;
			UnitOfWork = unitOfWork;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<ResultBase<TResult>> ExecuteAsync(TArguments arguments)
		{
			var clock = new Stopwatch();

			try
			{
				clock.Start();
				var result = await Handler.ExecuteAsync(arguments);
				clock.Stop();

				await CreateLogAsync(clock.Elapsed, Handler, arguments, result);

				return result;
			}
			catch (Exception ex)
			{
				clock.Stop();

				await CreateLogAsync(clock.Elapsed, Handler, arguments, null, ex);

				throw;
			}
			finally
			{
				await UnitOfWork.SaveAsync();
			}
		}
		#endregion ExecuteAsync
	}
}
