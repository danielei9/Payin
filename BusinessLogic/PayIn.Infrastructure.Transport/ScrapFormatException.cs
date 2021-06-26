using System;

namespace PayIn.Infrastructure.Transport
{
	public class ScrapFormatException : ApplicationException
	{
		public string Owner { get; set; }
		public string Text { get; set; }

		public ScrapFormatException (string owner, long uid, string text)
			: base("Bad format " + owner + " scrach (uid " + uid + "): " + text)
		{
			Owner = owner;
			Text = text;
		}
	}
}
