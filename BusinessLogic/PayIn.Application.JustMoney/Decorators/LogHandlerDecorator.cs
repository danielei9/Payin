using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using PayIn.Infrastructure.JustMoney.Db;
using System;
using System.Threading.Tasks;

namespace PayIn.Application.JustMoney.Decorators
{
	public abstract class LogHandlerDecorator
	{
		private readonly ISessionData SessionData;
		private readonly string RelatedClass;
		private readonly string RelatedMethod;
		private readonly string RelatedId;

		#region Contructors
		public LogHandlerDecorator(
			ISessionData sessionData,
			string relatedClass,
			string relatedMethod,
			string relatedId
		)
		{
			SessionData = sessionData ?? throw new ArgumentNullException(nameof(sessionData));
			RelatedClass = relatedClass;
			RelatedMethod = relatedMethod;
			RelatedId = relatedId;
		}
		#endregion Contructors

		#region CreateLog
		protected async Task<Log> CreateLogAsync(TimeSpan elapsed, object handler, object arguments, object result, Exception exception = null)
		{
			using (var context = new JustMoneyContext())
			{
				var message = "";
				if (exception != null)
				{
					var ex = exception;
					while (ex != null)
					{
						message += "\n" + ex.Message;
						ex = ex.InnerException;
					}
				}

				var log = new Log
				{
					DateTime = DateTime.Now.ToUTC(),
					Duration = elapsed,
					Login = SessionData.Login ?? "",
                    ClientId = SessionData.ClientId,
                    RelatedClass = RelatedClass,
					RelatedMethod = RelatedMethod,
					RelatedId = 0,
					Error = exception == null ? "" : message + "\n" + exception.StackTrace
				};
				context.Log.Add(log);

				foreach (var arg in arguments.GetType().GetProperties())
				{
					var value = arg.GetValue(arguments);
					if (value != null)
					{
						if ((arg.Name.ToLower() == RelatedId.ToLower()) && (value != null))
						{
							if (typeof(long?).IsAssignableFrom(arg.PropertyType))
								log.RelatedId = (value as long?).Value;
							else if (typeof(long).IsAssignableFrom(arg.PropertyType))
								log.RelatedId = (long)value;
							else if (typeof(int?).IsAssignableFrom(arg.PropertyType))
								log.RelatedId = (value as int?).Value;
							else if (typeof(int).IsAssignableFrom(arg.PropertyType))
								log.RelatedId = (int)value;
							else if (typeof(string).IsAssignableFrom(arg.PropertyType))
							{
								long val;
								if (long.TryParse(value as string, out val))
									log.RelatedId = val;
							}
						}

						log.Arguments.Add(new LogArgument
						{
							Name = arg.Name,
							Value = value.GetType().IsValueType ?
								value.ToString() :
								value.ToJson()
						});
					}
				}

				log.Results.Add(new LogResult
				{
					Name = "Result",
					Value = result == null ? "" : result.ToJson()
				});

				await context.SaveChangesAsync();

				return log;
			}
		}
		#endregion CreateLog
	}
}
