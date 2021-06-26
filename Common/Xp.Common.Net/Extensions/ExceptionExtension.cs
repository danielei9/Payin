namespace System
{
	public static class ExceptionExtension
	{
		#region GetXpMessage
		public static string GetXpMessage(this Exception exception)
		{
			if (exception is NotImplementedException)
				return exception.Message;
			if (exception is UnauthorizedAccessException)
				return exception.Message; // TODO: No se va a enviar el mensaje por temas de seguridad
			if (exception is ArgumentException)
				return exception.Message;
			if (exception is ArgumentNullException)
				return exception.Message;
			if (exception is ApplicationException)
				return exception.Message;

			return "La operación no se ha realizado con éxito";
		}
		#endregion GetXpMessage
	}
}

