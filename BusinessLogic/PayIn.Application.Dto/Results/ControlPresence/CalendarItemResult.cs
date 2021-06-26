using System;
using Xp.Common;

namespace PayIn.Domain.Public
{
	public class CalendarItemResult
	{
		public enum Types
		{
			Planning = 0,
			Presence = 1,
			Check = 2
		}

		public Types      Type     { get; set; }
		public string     Title    { get; set; }
		public string     Location { get; set; }
		public string     Info     { get; set; }
		public int        ItemId   { get; set; }
		public XpDateTime Start    { get; set; }
		public XpDateTime End      { get; set; }
		public XpDuration Duration
		{
			get
			{
				if ((Start == null) || (End == null))
					return TimeSpan.Zero;
				return End.Value.Subtract(Start.Value);
			}
		}
	}
}
