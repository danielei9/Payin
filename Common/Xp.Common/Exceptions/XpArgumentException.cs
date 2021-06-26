using System;

namespace Xp.Common.Exceptions
{
	public class XpArgumentException : XpException
	{
		public string ArgumentName { get; set; }
		public string ArgumentMessage { get; set; }

		public XpArgumentException(string argumentName, string argumentMessage)
			: base(string.Format("{1} ({0})", argumentName, argumentMessage))
		{
			ArgumentName = argumentName;
			ArgumentMessage = argumentMessage;
		}
		public XpArgumentException(string code, string argumentName, string argumentMessage)
			: base(code, string.Format("{1} ({0})", argumentName, argumentMessage, code))
		{
			ArgumentName = argumentName;
			ArgumentMessage = argumentMessage;
		}
		public XpArgumentException(string argumentName, string argumentMessage, Exception innerException)
			: base(string.Format("{1} ({0})", argumentName, argumentMessage), innerException)
		{
			ArgumentName = argumentName;
			ArgumentMessage = argumentMessage;
		}
		public XpArgumentException(string code, string argumentName, string argumentMessage, Exception innerException)
			: base(code, string.Format("{1} ({0})", argumentName, argumentMessage, innerException, code))
		{
			ArgumentName = argumentName;
			ArgumentMessage = argumentMessage;
		}
	}
}
