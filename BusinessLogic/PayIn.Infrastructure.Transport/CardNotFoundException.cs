using System;

namespace PayIn.Infrastructure.Transport
{
	public class CardNotFoundException : ApplicationException
	{
		public string Owner { get; set; }

		public CardNotFoundException(string owner)
			: base()
		{
			Owner = owner;
		}
	}
}
