using PayIn.Common;
using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class TicketCreateAndGetIFrameArguments : MobileTicketCreateAndGetArguments
	{
		public string Email { get; set; }
		public string Login { get; set; }

		#region Constructor
		public TicketCreateAndGetIFrameArguments(string email, string login, string reference, long? uid, DateTime date, int concessionId, int? eventId, IEnumerable<MobileTicketCreateAndGetArguments_TicketLine> lines, int? liquidationConcession)
            : base (reference, uid, date, concessionId, eventId, liquidationConcession, null, null, TicketType.Ticket, lines, null, null)
        {
			Email = email;
			Login = login;
		}
		#endregion Constructor
	}
}
