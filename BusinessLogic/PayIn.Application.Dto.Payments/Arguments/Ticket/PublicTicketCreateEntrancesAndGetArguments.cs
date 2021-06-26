using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class PublicTicketCreateEntrancesAndGetArguments : ICreateArgumentsBase<Ticket>
    {
        public string Email { get; set; }
        [Required]
        public string Login { get; set; }
        public string Reference { get; set; }
        [Required]
        public int ConcessionId { get; set; }
        public int? EventId { get; set; }
        public TicketType Type { get; set; }
        public IEnumerable<PublicTicketCreateEntrancesAndGetArguments_TicketLine> Lines { get; set; }
        [Required]
        public XpDateTime Now { get; set; }

        #region Constructor
        public PublicTicketCreateEntrancesAndGetArguments(
            string email,
            string login,
            string reference,
            int concessionId,
            int? eventId,
            IEnumerable<PublicTicketCreateEntrancesAndGetArguments_TicketLine> lines,
            TicketType? type
#if DEBUG
            , XpDateTime now
#endif //DEBUG
        )
        {
			Email = email ?? login; // Machacar mail de forma provisional
			Login = login;
			Reference    = reference ?? "";
			ConcessionId = concessionId;
			EventId = eventId;
			Lines  = lines ?? new List<PublicTicketCreateEntrancesAndGetArguments_TicketLine>();
			Type = type ?? TicketType.Ticket;
#if DEBUG
            Now = now ?? DateTime.UtcNow;
#else //DEBUG
			Now = DateTime.UtcNow;
#endif //DEBUG
        }
        #endregion Constructor
    }
}
