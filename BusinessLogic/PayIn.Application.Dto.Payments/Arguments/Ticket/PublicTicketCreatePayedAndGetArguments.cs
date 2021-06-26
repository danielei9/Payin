using PayIn.Common;
using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PublicTicketCreatePayedAndGetArguments : PublicTicketCreateAndGetArguments
	{
        public int TicketId { get; set; }
        public string AuthorizationCode { get; set; }
		public string CommerceCode { get; set; }

		#region Constructor
		public PublicTicketCreatePayedAndGetArguments(string authorizationCode, string commerceCode, string email, string login, string externalLogin, string reference, DateTime date, int concessionId, int? eventId, IEnumerable<MobileTicketCreateAndGetArguments_TicketLine> lines)
			:base(email, login, externalLogin, reference, date, concessionId, eventId, lines, TicketType.NotPayable)
		{
			AuthorizationCode = authorizationCode;
			CommerceCode = commerceCode;
		}
		#endregion Constructor
	}
}
