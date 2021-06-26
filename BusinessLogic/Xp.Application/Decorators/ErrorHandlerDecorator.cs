using System;

namespace Xp.Application.Decorators
{
	public abstract class ErrorHandlerDecorator
	{
		#region FilterException
		public Exception FilterException(Exception exception)
		{
			if (exception.GetType().Name == "HttpRequestException")
				return exception.InnerException;

			// Return null if exception don't change
			return null;
		}
		#endregion FilterException
	}
}
