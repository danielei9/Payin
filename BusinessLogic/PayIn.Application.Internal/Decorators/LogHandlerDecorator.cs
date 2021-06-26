using PayIn.BusinessLogic.Common;
using PayIn.Domain.Internal;
using PayIn.Infrastructure.Internal.Db;
using System;
using System.Threading.Tasks;
using Xp.Domain;

namespace PayIn.Application.Internal.Decorators
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
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			SessionData = sessionData;
			RelatedClass = relatedClass;
			RelatedMethod = relatedMethod;
			RelatedId = relatedId;
		}
		#endregion Contructors

		#region CreateLog
		protected async Task<Log> CreateLogAsync(TimeSpan elapsed, object handler, object arguments, object result, Exception exception = null)
		{
			using (var context = new InternalContext())
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
					Error = exception == null ? "" : message + "\n" + exception.StackTrace,
					Result = result == null ? "" : result.ToJson()
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

				await context.SaveChangesAsync();

				return log;
			}
		}
		#endregion CreateLog
	}
}
