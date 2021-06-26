using PayIn.Common;
using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketTpvCreateArguments : TicketMobileCreateArguments
	{
		#region Constructor
		public TicketTpvCreateArguments(string reference, DateTime date, IEnumerable<TicketMobileCreateArguments_TicketLine> lines, TicketType? type)
			:base(reference, date, 0, lines, null, type)
		{
		}
		#endregion Constructor
	}
}
