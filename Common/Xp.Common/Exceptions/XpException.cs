using System;

namespace Xp.Common.Exceptions
{
	public class XpException : Exception
	{
		public string Code { get; set; }

		#region Constructors
		public XpException()
		{
		}
		public XpException(string message)
			: base(message)
		{
		}
		public XpException(string code, string message)
			: base(message)
		{
			Code = code;
		}
		public XpException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
		public XpException(string code, string message, Exception innerException)
			: base(message, innerException)
		{
			Code = code;
		}
		#endregion Constructors
	}
}
