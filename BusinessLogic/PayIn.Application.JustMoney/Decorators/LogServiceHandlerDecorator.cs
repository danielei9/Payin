using PayIn.BusinessLogic.Common;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common.Dto.Arguments;
using Xp.Domain;

namespace PayIn.Application.JustMoney.Decorators
{
	public class LogServiceHandlerDecorator<TArguments> : LogHandlerDecorator, IServiceBaseHandler<TArguments>
		where TArguments : IArgumentsBase
	{
		public IServiceBaseHandler<TArguments> Handler { get; private set; }
		private readonly IUnitOfWork UnitOfWork;

		#region Contructors
		public LogServiceHandlerDecorator(
			IServiceBaseHandler<TArguments> handler,
			ISessionData sessionData,
			IUnitOfWork unitOfWork,
			string relatedClass,
			string relatedMethod,
			string relatedId
		)
			: base(sessionData, relatedClass, relatedMethod, relatedId)
		{
			Handler = handler ?? throw new ArgumentNullException(nameof(handler));
			UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TArguments arguments)
		{
			var clock = new Stopwatch();

			try
			{
				clock.Start();
				var result = await Handler.ExecuteAsync(arguments);
				clock.Stop();

				await CreateLogAsync(clock.Elapsed, Handler, arguments, null);

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
