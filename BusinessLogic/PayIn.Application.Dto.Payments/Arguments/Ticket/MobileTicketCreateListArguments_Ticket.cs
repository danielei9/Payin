using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class MobileTicketCreateListArguments_Ticket
	{
        public string Reference { get; set; } = "";
        public string ExternalLogin { get; set; }
        [Required]
        public XpDateTime Date { get; set; }
        public int? EventId { get; set; }
        public int? TransportPrice { get; set; }
        public TicketType Type { get; set; }
        public IEnumerable<MobileTicketCreateAndGetArguments_TicketLine> Lines { get; set; }
        public IEnumerable<MobileTicketCreateAndGetArguments_Payment> Payments { get; set; }
        public IEnumerable<TicketAnswerFormsArguments_Form> Forms { get; set; }
    }
}
